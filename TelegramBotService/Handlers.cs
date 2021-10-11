using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotService
{
    public delegate Task MessageHandler(ITelegramBotClient botClient, Message message);
    public delegate Task<Message> TextMessageHandler(ITelegramBotClient botClient, Message message);
    public delegate Task CallbackQueryHandler(ITelegramBotClient botClient, CallbackQuery callbackQuery);
    public delegate Task InlineQueryHandler(ITelegramBotClient botClient, InlineQuery inlineQuery);
    public delegate Task ChosenInlineResultHandler(ITelegramBotClient botClient, ChosenInlineResult chosenInlineResult);
    public delegate Task UnknownUpdateHandler(ITelegramBotClient botClient, Update update);
    public delegate Task ShippingQueryHandler(ITelegramBotClient botClient, ShippingQuery shippingQuery);
    public delegate Task PreCheckoutQueryHandler(ITelegramBotClient botClient, PreCheckoutQuery preCheckoutQuery);
    public delegate Task PollHandler(ITelegramBotClient botClient, Poll poll);

    public class Handlers : ITelegramHandlers
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

        public Dictionary<string, TextMessageHandler> MessageHandlers = new Dictionary<string, TextMessageHandler>()
        {
            { "/inline_mode",  (ITelegramBotClient botClient, Message message) => SendInlineKeyboard(botClient, message)},
            { "/keyboard", (ITelegramBotClient botClient, Message message) => SendReplyKeyboard(botClient, message)},
            { "/remove_keyboard", (ITelegramBotClient botClient, Message message) => RemoveKeyboard(botClient, message)},
            { "/photo", (ITelegramBotClient botClient, Message message) => SendFile(botClient, message)},
            { "/request", (ITelegramBotClient botClient, Message message) => RequestContactAndLocation(botClient, message)},
        };

        public List<string> Users = new List<string>()
        {
            "MagnaPhysicus"
        };

        private readonly ILogger<Handlers> _logger;

        public Handlers(ILogger<Handlers> logger)
        {
            _logger = logger;
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
                access = Users.Contains(message.From.Username);
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
            foreach (var method in MessageHandlers)
            {
                if (command == method.Key)
                {
                    action = method.Value(botClient, message);
                }
            }
            if (action == null)
            {
                action = Usage(botClient, message);
            }
            var messageResult = await action;
            _logger.LogInformation($"The message was sent with id: {messageResult.MessageId}");
        }

        static async Task<Message> SendInlineKeyboard(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("1.1", "11"),
                        InlineKeyboardButton.WithCallbackData("1.2", "12"),
                    },
                    // second row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("2.1", "21"),
                        InlineKeyboardButton.WithCallbackData("2.2", "22"),
                    },
                });

            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                        text: "Choose",
                                                        replyMarkup: inlineKeyboard);
        }

        static async Task<Message> SendReplyKeyboard(ITelegramBotClient botClient, Message message)
        {
            var replyKeyboardMarkup = new ReplyKeyboardMarkup(
                new KeyboardButton[][]
                {
                        new KeyboardButton[] { "1.1", "1.2" },
                        new KeyboardButton[] { "2.1", "2.2" },
                })
            {
                ResizeKeyboard = true
            };

            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                        text: "Choose",
                                                        replyMarkup: replyKeyboardMarkup);
        }

        static async Task<Message> RemoveKeyboard(ITelegramBotClient botClient, Message message)
        {
            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                        text: "Removing keyboard",
                                                        replyMarkup: new ReplyKeyboardRemove());
        }

        static async Task<Message> SendFile(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

            const string filePath = @"Files/tux.png";
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var fileName = filePath.Split(Path.DirectorySeparatorChar).Last();

            return await botClient.SendPhotoAsync(chatId: message.Chat.Id,
                                                  photo: new InputOnlineFile(fileStream, fileName),
                                                  caption: "Nice Picture");
        }

        static async Task<Message> RequestContactAndLocation(ITelegramBotClient botClient, Message message)
        {
            var RequestReplyKeyboard = new ReplyKeyboardMarkup(new[]
            {
                    KeyboardButton.WithRequestLocation("Location"),
                    KeyboardButton.WithRequestContact("Contact"),
                    KeyboardButton.WithRequestPoll("Poll"),
                });

            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                        text: "Who or Where are you?",
                                                        replyMarkup: RequestReplyKeyboard);
        }

        static async Task<Message> Usage(ITelegramBotClient botClient, Message message)
        {
            const string usage = "Usage:\n" +
                                 "/inline_mode   - send inline keyboard\n" +
                                 "/keyboard - send custom keyboard\n" +
                                 "/remove_keyboard   - remove custom keyboard\n" +
                                 "/photo    - send a photo\n" +
                                 "/request  - request location or contact";

            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                        text: usage,
                                                        replyMarkup: new ReplyKeyboardRemove());
        }

        // Process Inline Keyboard callback data
        private async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"Received {callbackQuery.Data}");

            await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: $"Received {callbackQuery.Data}");
        }

        private async Task BotOnInlineQueryReceived(ITelegramBotClient botClient, InlineQuery inlineQuery)
        {
            _logger.LogInformation($"Received inline query from: {inlineQuery.From.Id}");

            InlineQueryResultBase[] results = {
                // displayed result
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
