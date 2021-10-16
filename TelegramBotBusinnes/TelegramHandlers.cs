using Domain.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using TelegramBotService;

namespace TelegramBotBusiness
{
    public class TelegramHandlers : AbstractTelegramHandlers
    {
        public TelegramHandlers(
            Option[] options,
            TelegramUser[] users,
            ILogger<AbstractTelegramHandlers> logger,
            ITelegramConfiguration configuration)
            : base(users, logger, configuration)
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
            var words = message.Text.Split(new char[] { '?' }, StringSplitOptions.RemoveEmptyEntries);

            if (words[0].Contains('@') && words[0].Contains(name) == false)
                return;

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

        protected override async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            var callbackQueryHandler = CallbackQueryHandlers.FirstOrDefault(c => c.Command == callbackQuery.Data);
            if (callbackQueryHandler != null)
            {
                PendingInput = await callbackQueryHandler.Handler(botClient, callbackQuery);
            }
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
