using GoogleCalendarService;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotService;

namespace TelegramBotBusiness.CallbackQueryHandlers
{
    public class OutputCallbackQueryHandler
    {
        private readonly IGoogleCalendar _googleCalendar;

        public OutputCallbackQueryHandler(IGoogleCalendar googleCalendar)
        {
            _googleCalendar = googleCalendar;
        }

        public async Task<MessageHandlerReturningMessage> BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: callbackQuery.Data);

            await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: callbackQuery.Data);
            return null;
        }

        public async Task<MessageHandlerReturningMessage> BotOnGetAllEventsReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            var events = await _googleCalendar.ShowUpCommingEvents();
            await botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: events);

            await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: events);
            return null;
        }

        public async Task<MessageHandlerReturningMessage> BotOnGetFilteredEventsReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: "Введите заголовок или описание события для фильтрации");

            await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: "Введите заголовок или описание события для фильтрации");
            MessageHandlerReturningMessage messageHandler = WaitForThePropertyToBeEntered;
            return messageHandler;
        }

        private async Task<Message> WaitForThePropertyToBeEntered(ITelegramBotClient botClient, Message message)
        {
            var events = await _googleCalendar.GetEvents(q: message.Text);
            var textMessage = await _googleCalendar.ShowUpCommingEvents(events);
            return await botClient.SendTextMessageAsync(message.Chat.Id, textMessage);
        }
    }
}
