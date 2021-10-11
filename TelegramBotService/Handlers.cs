using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotService;

namespace TelegramBotBusiness
{
    
    public class Handlers : ITelegramHandler
    {
        public event MessageHandler MessageNotify;
        public event CallbackQueryHandler CallbackQueryNotify;
        public event InlineQueryHandler InlineQueryNotify;
        public event ChosenInlineResultHandler ChosenInlineResultNotify;
        public event UnknownUpdateHandler UnknownUpdateNotify;

        public event MessageHandler ChannelPostNotify;
        public event MessageHandler EditedChannelPostNotify;
        public event ShippingQueryHandler ShippingQueryNotify;
        public event PreCheckoutQueryHandler PreCheckoutQueryNotify;
        public event PollHandler PollNotify;

        public List<TextMessageHandler> TextMessageHandlers;
        public List<CallbackQueryMessageHandler> CallbackQueryHandlers;

        public readonly ILogger<Handlers> _logger;
        private readonly TelegramOptions _options;

        public Handlers(ILogger<Handlers> logger, IOptions<TelegramOptions> options)
        {
            _logger = logger;
            _options = options.Value;
            HandlerConfiguration.Configuration(this);
            MessageNotify = BotOnMessageReceived;
            CallbackQueryNotify = BotOnCallbackQueryReceived;
            InlineQueryNotify = BotOnInlineQueryReceived;
            ChosenInlineResultNotify = BotOnChosenInlineResultReceived;
            UnknownUpdateNotify = UnknownUpdateHandlerAsync;
        }

        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogError(ErrorMessage);
            return Task.CompletedTask;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;
            bool access = true;
            if (message != null)
            {
                access = _options.Users.Contains(message.From.Username);
            }
            if (access)
            {
                var handler = update.Type switch
                {
                    UpdateType.ChannelPost => ChannelPostNotify?.Invoke(botClient, update.ChannelPost),
                    UpdateType.EditedChannelPost => EditedChannelPostNotify?.Invoke(botClient, update.EditedChannelPost),
                    UpdateType.ShippingQuery => ShippingQueryNotify?.Invoke(botClient, update.ShippingQuery),
                    UpdateType.PreCheckoutQuery => PreCheckoutQueryNotify?.Invoke(botClient, update.PreCheckoutQuery),
                    UpdateType.Poll => PollNotify?.Invoke(botClient, update.Poll),

                    UpdateType.Message => MessageNotify?.Invoke(botClient, update.Message),
                    UpdateType.EditedMessage => MessageNotify?.Invoke(botClient, update.EditedMessage),
                    UpdateType.CallbackQuery => CallbackQueryNotify?.Invoke(botClient, update.CallbackQuery),
                    UpdateType.InlineQuery => InlineQueryNotify?.Invoke(botClient, update.InlineQuery),
                    UpdateType.ChosenInlineResult => ChosenInlineResultNotify?.Invoke(botClient, update.ChosenInlineResult),
                    _ => UnknownUpdateNotify?.Invoke(botClient, update)
                };
                try
                {
                    await handler;
                }
                catch (Exception exception)
                {
                    await HandleErrorAsync(botClient, exception, cancellationToken);
                }
            }
        }

        private async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            _logger.LogInformation($"Receive message type: {message.Type}");
            if (message.Type != MessageType.Text)
                return;

            Task<Message> action = null;
            var me = await botClient.GetMeAsync();
            var name = me.Username;
            var words = message.Text.Split(new char[] { ' ', '@' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 2 && words[1] != me.Username)
            {
                return;
            }
            string command = words.First();
            foreach (var method in TextMessageHandlers)
            {
                if (command == method.Command)
                {
                    action = method.Handler(botClient, message);
                }
            }
            if (action == null)
            {
                action = Usage(botClient, message);
            }
            var messageResult = await action;
            _logger.LogInformation($"The message was sent with id: {messageResult.MessageId}");
        }

        private async Task<Message> Usage(ITelegramBotClient botClient, Message message)
        {
            string usage = "Usage:\n";
            foreach (var command in TextMessageHandlers)
            {
                usage += $"{command.Command} - {command.Description}\n";
            }
            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                        text: usage,
                                                        replyMarkup: new ReplyKeyboardRemove());
        }

        private async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            var callbackQueryHandler = CallbackQueryHandlers.FirstOrDefault(c => c.Command == callbackQuery.Data);
            if(callbackQueryHandler != null)
            {
                await callbackQueryHandler.Handler(botClient, callbackQuery);
            }

            //await botClient.AnswerCallbackQueryAsync(
            //    callbackQueryId: callbackQuery.Id,
            //    text: $"Received {callbackQuery.Data}");

            //await botClient.SendTextMessageAsync(
            //    chatId: callbackQuery.Message.Chat.Id,
            //    text: $"Received {callbackQuery.Data}");
        }

        private async Task BotOnInlineQueryReceived(ITelegramBotClient botClient, InlineQuery inlineQuery)
        {
            _logger.LogInformation($"Received inline query from: {inlineQuery.From.Id}");

            InlineQueryResultBase[] results = {
                new InlineQueryResultArticle(
                    id: "3",
                    title: "TgBots",
                    inputMessageContent: new InputTextMessageContent(
                        "hello"
                    )
                )
            };

            await botClient.AnswerInlineQueryAsync(
                inlineQueryId: inlineQuery.Id,
                results: results,
                isPersonal: true,
                cacheTime: 0);
        }

        private Task BotOnChosenInlineResultReceived(ITelegramBotClient botClient, ChosenInlineResult chosenInlineResult)
        {
            _logger.LogInformation($"Received inline result: {chosenInlineResult.ResultId}");
            return Task.CompletedTask;
        }

        private Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {
            _logger.LogInformation($"Unknown update type: {update.Type}");
            return Task.CompletedTask;
        }
    }
}
