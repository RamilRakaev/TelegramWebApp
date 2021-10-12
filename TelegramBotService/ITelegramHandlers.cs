using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.Payments;
using System.Linq;
using Microsoft.Extensions.Options;
using Telegram.Bot.Exceptions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace TelegramBotService
{
    public delegate Task MessageHandler(ITelegramBotClient botClient, Message message);
    public delegate Task<Message> MessageHandlerReturningMessage(ITelegramBotClient botClient, Message message);
    public delegate Task CallbackQueryHandler(ITelegramBotClient botClient, CallbackQuery callbackQuery);
    public delegate Task InlineQueryHandler(ITelegramBotClient botClient, InlineQuery inlineQuery);
    public delegate Task ChosenInlineResultHandler(ITelegramBotClient botClient, ChosenInlineResult chosenInlineResult);
    public delegate Task UnknownUpdateHandler(ITelegramBotClient botClient, Update update);
    public delegate Task ShippingQueryHandler(ITelegramBotClient botClient, ShippingQuery shippingQuery);
    public delegate Task PreCheckoutQueryHandler(ITelegramBotClient botClient, PreCheckoutQuery preCheckoutQuery);
    public delegate Task PollHandler(ITelegramBotClient botClient, Poll poll);
    public delegate Task PollAnswerHandler(ITelegramBotClient botClient, PollAnswer pollAnswer);
    public delegate Task ChatMemberUpdatedHandler(ITelegramBotClient botClient, ChatMemberUpdated chatMemberUpdated);

    public abstract class ITelegramHandlers
    {
        protected readonly TelegramOptions _options;
        protected readonly ILogger<ITelegramHandlers> _logger;
        public List<TextMessageHandler> TextMessageHandlers;
        public List<CallbackQueryMessageHandler> CallbackQueryHandlers;

        public ITelegramHandlers(ILogger<ITelegramHandlers> logger,
            IOptions<TelegramOptions> options,
            ITelegramConfiguration configuration)
        {
            _logger = logger;
            _options = options.Value;
            configuration.Configurate(this);
        }

        protected event MessageHandler ChannelPostNotify;
        protected event MessageHandler EditedChannelPostNotify;
        protected event ShippingQueryHandler ShippingQueryNotify;
        protected event PreCheckoutQueryHandler PreCheckoutQueryNotify;
        protected event PollHandler PollNotify;
        protected event PollAnswerHandler PollAnswerNotify;
        protected event ChatMemberUpdatedHandler MyChatMemberUpdatedNotify;
        protected event ChatMemberUpdatedHandler ChatMemberUpdatedNotify;

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
                if(access == false)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, "У вас нет прав для использования бота");
                }
            }
            if (access)
            {
                var handler = update.Type switch
                {
                    UpdateType.ChannelPost => ChannelPostNotify?.Invoke(botClient, update.ChannelPost),
                    UpdateType.EditedChannelPost => EditedChannelPostNotify?.Invoke(botClient, update.EditedChannelPost),
                    UpdateType.ShippingQuery => ShippingQueryNotify?.Invoke(botClient, update.ShippingQuery),
                    UpdateType.PreCheckoutQuery => PreCheckoutQueryNotify?.Invoke( botClient, update.PreCheckoutQuery),
                    UpdateType.Poll => PollNotify?.Invoke(botClient, update.Poll),
                    UpdateType.PollAnswer => PollAnswerNotify?.Invoke(botClient, update.PollAnswer),
                    UpdateType.MyChatMember => MyChatMemberUpdatedNotify?.Invoke(botClient, update.MyChatMember),
                    UpdateType.ChatMember => ChatMemberUpdatedNotify?.Invoke(botClient, update.ChatMember),

                    UpdateType.Message => BotOnMessageReceived(botClient, update.Message),
                    UpdateType.EditedMessage => BotOnMessageReceived(botClient, update.EditedMessage),
                    UpdateType.CallbackQuery => BotOnCallbackQueryReceived(botClient, update.CallbackQuery),
                    UpdateType.InlineQuery => BotOnInlineQueryReceived(botClient, update.InlineQuery),
                    UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(botClient, update.ChosenInlineResult),
                    _ => UnknownUpdateHandlerAsync(botClient, update)
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

        protected abstract Task BotOnMessageReceived(ITelegramBotClient botClient, Message message);

        protected abstract Task<Message> Usage(ITelegramBotClient botClient, Message message);

        protected abstract Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery);

        protected abstract Task BotOnInlineQueryReceived(ITelegramBotClient botClient, InlineQuery inlineQuery);

        protected abstract Task BotOnChosenInlineResultReceived(ITelegramBotClient botClient, ChosenInlineResult chosenInlineResult);

        protected abstract Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update);

    }
}
