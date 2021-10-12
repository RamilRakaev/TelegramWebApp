using GoogleCalendarService;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotBusiness.CallbackQueryHandlers
{
    public class OutputCallbackQueryHandler
    {
        private readonly IGoogleCalendar _googleCalendar;

        public OutputCallbackQueryHandler(IGoogleCalendar googleCalendar)
        {
            _googleCalendar = googleCalendar;
        }

        public async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: callbackQuery.Data);

            await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: callbackQuery.Data);
        }

        public async Task BotOnGetAllEventsReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            var events = await _googleCalendar.ShowUpCommingEvents();
            await botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: "/filtered_events");

            await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: events);
        }

        public async Task BotOnGetFilteredEventsReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: "Введите заголовок или описание события для фильтрации");

            await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: "Введите заголовок или описание события для фильтрации");
        }
    }
}
