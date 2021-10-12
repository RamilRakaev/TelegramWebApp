using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotService;

namespace TelegramBotBusiness
{

    public class Handlers : ITelegramHandlers
    {
        

        public Handlers(ILogger<Handlers> logger, IOptions<TelegramOptions> options, ITelegramConfiguration configuration) 
            : base(logger, options, configuration)
        {
        }

        protected override async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message)
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

        protected override async Task<Message> Usage(ITelegramBotClient botClient, Message message)
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

        protected override async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            var callbackQueryHandler = CallbackQueryHandlers.FirstOrDefault(c => c.Command == callbackQuery.Data);
            if (callbackQueryHandler != null)
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

        protected override async Task BotOnInlineQueryReceived(ITelegramBotClient botClient, InlineQuery inlineQuery)
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

        protected override Task BotOnChosenInlineResultReceived(ITelegramBotClient botClient, ChosenInlineResult chosenInlineResult)
        {
            _logger.LogInformation($"Received inline result: {chosenInlineResult.ResultId}");
            return Task.CompletedTask;
        }

        protected override Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {
            _logger.LogInformation($"Unknown update type: {update.Type}");
            return Task.CompletedTask;
        }
    }
}
