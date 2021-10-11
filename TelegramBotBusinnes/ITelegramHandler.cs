using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;

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

    public interface ITelegramHandler
    {
        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken);

        Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
    }
}
