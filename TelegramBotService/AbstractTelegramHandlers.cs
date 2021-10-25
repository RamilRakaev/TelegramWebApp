using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Linq;
using Telegram.Bot.Exceptions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotService
{
    public delegate Task MessageHandler(ITelegramBotClient botClient, Message message);
    public delegate Task<Message> MessageHandlerReturningMessage(ITelegramBotClient botClient, Message message);
    public delegate Task<MessageHandlerReturningMessage> CallbackQueryHandler(ITelegramBotClient botClient, CallbackQuery callbackQuery);
    public delegate Task InlineQueryHandler(ITelegramBotClient botClient, InlineQuery inlineQuery);

    public abstract class AbstractTelegramHandlers
    {
        protected readonly string[] _users;
        protected readonly ILogger<AbstractTelegramHandlers> _logger;
        public List<TextMessageCommandHandler> textMessageCommandHandlers;
        public List<CallbackQueryCommandHandler> callbackQueryCommandHandlers;
        public List<InlineQueryCommandHandler> inlineQueryCommandHandlers;
        protected MessageHandlerReturningMessage PendingInput;

        public AbstractTelegramHandlers(
            string[] users,
            ILogger<AbstractTelegramHandlers> logger,
            ITelegramHandlerConfiguration configuration)
        {
            _users = users;
            _logger = logger;
            configuration.Configurate(this);
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

        private async Task<bool> RightsVerification(ITelegramBotClient botClient, Update update)
        {
            var message = update.Message;
            bool access = true;
            if (message != null)
            {
                access = _users.Contains(message.From.Username);
                if (access == false)
                {
                    await botClient.SendTextMessageAsync(chatId: message.Chat.Id, "У вас нет прав для использования бота");
                }
            }
            return access;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            bool access = await RightsVerification(botClient, update);
            if (access)
            {
                var handler = update.Type switch
                {
                    //UpdateType.ChannelPost => ChannelPostNotify?.Invoke(botClient, update.ChannelPost),
                    //UpdateType.EditedChannelPost => EditedChannelPostNotify?.Invoke(botClient, update.EditedChannelPost),
                    //UpdateType.ShippingQuery => ShippingQueryNotify?.Invoke(botClient, update.ShippingQuery),
                    //UpdateType.PreCheckoutQuery => PreCheckoutQueryNotify?.Invoke(botClient, update.PreCheckoutQuery),
                    //UpdateType.Poll => PollNotify?.Invoke(botClient, update.Poll),
                    //UpdateType.PollAnswer => PollAnswerNotify?.Invoke(botClient, update.PollAnswer),
                    //UpdateType.MyChatMember => MyChatMemberUpdatedNotify?.Invoke(botClient, update.MyChatMember),
                    //UpdateType.ChatMember => ChatMemberUpdatedNotify?.Invoke(botClient, update.ChatMember),

                    UpdateType.Message => ProcessingTextMessages(botClient, update.Message),
                    UpdateType.EditedMessage => ProcessingTextMessages(botClient, update.EditedMessage),
                    UpdateType.CallbackQuery => BotOnCallbackQueryReceived(botClient, update.CallbackQuery),
                    UpdateType.InlineQuery => BotOnInlineQueryReceived(botClient, update.InlineQuery),
                    UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(botClient, update.ChosenInlineResult),
                    _ => UnknownUpdateHandlerAsync(botClient, update)
                };
                try
                {
                    if (handler != null)
                    {
                        await handler;
                    }
                }
                catch (Exception exception)
                {
                    await HandleErrorAsync(botClient, exception, cancellationToken);
                }
            }
        }

        private async Task ProcessingTextMessages(ITelegramBotClient botClient, Message message)
        {
            if (message.ViaBot == null)
            {
                if (PendingInput != null)
                {
                    await PendingInput(botClient, message);
                    PendingInput = null;
                }
                else
                {
                    await BotOnMessageReceived(botClient, message);
                }
            }

        }

        protected async Task<Message> Usage(ITelegramBotClient botClient, Message message)
        {
            string usage = "Commands:\n";
            foreach (var command in textMessageCommandHandlers)
            {
                usage += $"\"{command.Command}\" - {command.Description}\n";
            }
            usage += "\nInline queries:\n";
            foreach (var command in inlineQueryCommandHandlers)
            {
                usage += $"\"{command.Command}\" - {command.Description}\n";
            }
            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                        text: usage,
                                                        replyMarkup: new ReplyKeyboardRemove());
        }

        protected abstract Task BotOnMessageReceived(ITelegramBotClient botClient, Message message);

        protected abstract Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery);

        protected abstract Task BotOnInlineQueryReceived(ITelegramBotClient botClient, InlineQuery inlineQuery);

        protected abstract Task BotOnChosenInlineResultReceived(ITelegramBotClient botClient, ChosenInlineResult chosenInlineResult);

        protected abstract Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update);

    }
}
