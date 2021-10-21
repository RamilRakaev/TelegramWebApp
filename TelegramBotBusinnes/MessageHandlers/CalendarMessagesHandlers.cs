using GoogleCalendarService;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotBusiness.MessageHandlers
{
    public class CalendarMessagesHandlers
    {
        private readonly IGoogleCalendar _googleCalendar;

        public CalendarMessagesHandlers(IGoogleCalendar googleCalendar)
        {
            _googleCalendar = googleCalendar;
        }

        public async Task<Message> SendFilteredCalendarEvents(ITelegramBotClient botClient, Message message)
        {
            var words = message.Text.Split(new char[] { '?', '@' }, StringSplitOptions.RemoveEmptyEntries);
            string text;
            if (words.Length != 2)
            {
                text = "Enter the property by which the elements will be filtered: /filtered_events?(property)";
            }
            else
            {
                var events = await _googleCalendar.GetEvents(q: words[1]);
                text = await _googleCalendar.ShowUpCommingEvents(events);
            }
            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                        text: text,
                                                        replyMarkup: new ReplyKeyboardRemove());
        }

        public async Task<Message> SendAllCalendarEvents(ITelegramBotClient botClient, Message message)
        {
            var text = await _googleCalendar.ShowUpCommingEvents();

            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                        text: text,
                                                        replyMarkup: new ReplyKeyboardRemove());
        }

        public async Task<Message> SendEventsInTimeInterval(ITelegramBotClient botClient, Message message)
        {
            try
            {
                var text = message.Text.Split('?').Last();
                int startHours = Convert.ToInt32(text.Substring(0, 2));
                int startMinutes = Convert.ToInt32(text.Substring(3, 2));
                int endHours = Convert.ToInt32(text.Substring(6, 2));
                int endMinutes = Convert.ToInt32(text.Substring(9, 2));
                var textMessage = await _googleCalendar.ShowDayEventsInTimeInterval(startHours, startMinutes, endHours, endMinutes);


                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: textMessage,
                                                            replyMarkup: new ReplyKeyboardRemove());
            }
            catch
            {
                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                               text: "Input error",
                                                               replyMarkup: new ReplyKeyboardRemove());
            }
        }

        public async Task<Message> SendEventsInDateTimeInterval(ITelegramBotClient botClient, Message message)
        {
            try
            {
                //var text = message.Text[(message.Text.IndexOf('?') + 1)..].Split('-');
                //var startDateTime = Convert.ToDateTime(text[0]);
                //var endDateTime = Convert.ToDateTime(text[1]);
                //var events = await _googleCalendar.GetEvents(startDateTime, endDateTime);
                //var textMessage = await _googleCalendar.ShowUpCommingEvents(events);


                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: await _googleCalendar.EventsInDateTimeIntervalCommandHandler(message.Text),
                                                            replyMarkup: new ReplyKeyboardRemove());
            }
            catch
            {
                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                               text: "Input error",
                                                               replyMarkup: new ReplyKeyboardRemove());
            }
        }
    }
}
