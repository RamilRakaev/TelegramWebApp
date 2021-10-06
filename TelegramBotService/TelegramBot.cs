using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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

        public Dictionary<string, string> Text = new Dictionary<string, string>()
        {
            {"/saysomething", "тест"}
        };

        public void Start()
        {
            cts = new CancellationTokenSource();
            bot.StartReceiving(
                new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync),
                cts.Token);
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
            if (message.Type == MessageType.Text)
            {
                await ProcessTextMessage(message);
                //if (message.Text == "/saysomething")
                //{
                //    await bot.SendTextMessageAsync(message.Chat.Id, "тест",
                //           replyToMessageId: message.MessageId);
                //}
            }
            return "";
        }

        private async Task ProcessTextMessage(Message message)
        {
            if (Text.ContainsKey(message.Text))
            {
                await bot.SendTextMessageAsync(
                    message.Chat.Id,
                    Text[message.Text],
                    replyToMessageId: message.MessageId);
            }
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
            var chatId = update.Message.Chat.Id;
            var text = await AnswerAsync(update);
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: text
            );
        }
    }
}
