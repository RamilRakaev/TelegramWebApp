using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotService
{
    public class TelegramBot
    {
        private readonly TelegramBotClient bot;
        private CancellationTokenSource cts;

        public TelegramBot(string apiKey)
        {
            bot = new TelegramBotClient(apiKey);
        }

        public List<TelegramText> TelegramText = new List<TelegramText>()
        {
            new TelegramText("/saysomething", "просто текст", "тест")
        };

        public List<TelegramImage> TelegramImages = new List<TelegramImage>()
        {
            new TelegramImage("/getimage",
                "http://aftamat4ik.ru/wp-content/uploads/2017/03/photo_2016-12-13_23-21-07.jpg",
                "Revolution!")
        };

        public List<TelegramInlineButton> TelegramInlineButtons = new List<TelegramInlineButton>()
        {
            new TelegramInlineButton("/inlinebutton",
                new List<TelegramCallbackQuery>(){

                    new TelegramCallbackQuery("callback1", "300", "О***** у тракториста"),
                    new TelegramCallbackQuery("callback2", "100", "Х** в очко")


                })
        };

        public void Start()
        {
            cts = new CancellationTokenSource();
            bot.StartReceiving(
                new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync),
                cts.Token);
            bot.OnCallbackQuery += async (object sc, Telegram.Bot.Args.CallbackQueryEventArgs ev) =>
            {
                var message = ev.CallbackQuery.Message;
                if (ev.CallbackQuery.Data == "callback1")
                {
                    await bot.AnswerCallbackQueryAsync(ev.CallbackQuery.Id, "You hav choosen " + ev.CallbackQuery.Data, true);
                }
                else
                if (ev.CallbackQuery.Data == "callback2")
                {
                    await bot.SendTextMessageAsync(message.Chat.Id, "тест", replyToMessageId: message.MessageId);
                    await bot.AnswerCallbackQueryAsync(ev.CallbackQuery.Id); // отсылаем пустое, чтобы убрать "частики" на кнопке
                }
            };
        }


        public void Stop()
        {
            if (cts != null)
            {
                cts.Cancel();
            }
        }

        private async Task<string> AnswerAsync(Update update)
        {
            var message = update.Message;
            if (message != null && message.Type == MessageType.Text)
            {
                await ProcessTextMessage(message);
            }
            return "";
        }

        private async Task ProcessTextMessage(Message message)
        {
            var image = TelegramImages.FirstOrDefault(i => i.Command == message.Text);
            if (image != null)
            {
                await bot.SendPhotoAsync(
                    message.Chat.Id,
                    image.Source,
                    image.Title);
                goto FinishMethod;
            }
            var text = TelegramText.FirstOrDefault(i => i.Command == message.Text);
            if (text != null)
            {
                await bot.SendTextMessageAsync(
                    message.Chat.Id,
                    text.Text,
                    replyToMessageId: message.MessageId);
                goto FinishMethod;
            }
            if (message.Text == "/ibuttons")
            {
                var keyboard = new InlineKeyboardMarkup(
                    InlineKeyboardButton.WithCallbackData("300", "callback2")
                    );
                await bot.SendTextMessageAsync
                    (message.Chat.Id,
                    "Давай накатим, товарищ, по одной!",
                    ParseMode.Default,
                    replyMarkup: keyboard);
                goto FinishMethod;
            }
        FinishMethod:;
        }

        async Task<string> HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            return await Task.FromResult(errorMessage);
        }

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            await AnswerAsync(update);
        }
    }
}
