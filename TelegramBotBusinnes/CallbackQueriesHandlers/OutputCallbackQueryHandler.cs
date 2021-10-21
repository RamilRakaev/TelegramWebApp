using GoogleCalendarService;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotService;

namespace TelegramBotBusiness.CallbackQueriesHandlers
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
                text: "/all_events");

            await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: events);
            return null;
        }

        public async Task<MessageHandlerReturningMessage> BotOnGetFilteredEventsReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(
                 callbackQuery.Id,
                "Enter the name or description of the event to filter");

            await botClient.SendTextMessageAsync(
                callbackQuery.Message.Chat.Id,
                "Enter the name or description of the event to filter");
            return WaitForThePropertyToBeEntered;
        }

        private async Task<Message> WaitForThePropertyToBeEntered(ITelegramBotClient botClient, Message message)
        {
            var events = await _googleCalendar.GetEvents(q: message.Text);
            var textMessage = await _googleCalendar.ShowUpCommingEvents(events);
            return await botClient.SendTextMessageAsync(message.Chat.Id, textMessage);
        }

        public async Task<MessageHandlerReturningMessage> BotOnGetEventsInTimeIntervalReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(
                callbackQuery.Id,
                "Enter time interval for example: \"12:15-14:00\""
                );
            await botClient.SendTextMessageAsync(
                callbackQuery.Message.Chat.Id,
                "Enter time interval for example: \"12:15-14:00\"");
            return WaitForTheTimeIntervalToBeEntered;
        }

        private async Task<Message> WaitForTheTimeIntervalToBeEntered(ITelegramBotClient botClient, Message message)
        {
            try
            {
                var text = message.Text;
                int startHours = Convert.ToInt32(text.Substring(0, 2));
                int startMinutes = Convert.ToInt32(text.Substring(3, 2));
                int endHours = Convert.ToInt32(text.Substring(6, 2));
                int endMinutes = Convert.ToInt32(text.Substring(9, 2));
                var textMessage = await _googleCalendar.ShowDayEventsInTimeInterval(startHours, startMinutes, endHours, endMinutes);
                return await botClient.SendTextMessageAsync(message.Chat.Id, textMessage);
            }
            catch
            {
                return await botClient.SendTextMessageAsync(message.Chat.Id, "Input error");
            }
        }

        public async Task<MessageHandlerReturningMessage> BotOnGetEventsInDateTimeIntervalReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(
                callbackQuery.Id,
                "Enter time interval for example: \"10:15-19:23\""
                );
            await botClient.SendTextMessageAsync(
                callbackQuery.Message.Chat.Id,
                "Enter time interval for example: \"21.10.2021 12:15 - 24.10.2021 14:00\"");
            return WaitForTheDateTimeIntervalToBeEntered;
        }

        private async Task<Message> WaitForTheDateTimeIntervalToBeEntered(ITelegramBotClient botClient, Message message)
        {
            try
            {
                var text = message.Text.Split("-");
                var startDateTime = Convert.ToDateTime(text[0]);
                var endDateTime = Convert.ToDateTime(text[1]);
                var events = await _googleCalendar.GetEvents(startDateTime, endDateTime);
                var textMessage = await _googleCalendar.ShowUpCommingEvents(events);
                return await botClient.SendTextMessageAsync(message.Chat.Id, textMessage);
            }
            catch
            {
                return await botClient.SendTextMessageAsync(message.Chat.Id, "Input error");
            }
        }
    }
}
