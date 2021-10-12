using GoogleCalendarService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotBusiness.MessageHandlers
{
    public class CalendarHandlers
    {
        private readonly IGoogleCalendar _googleCalendar;

        public CalendarHandlers(IGoogleCalendar googleCalendar)
        {
            _googleCalendar = googleCalendar;
        }

        public async Task<Message> SendFilteredCalendarEvents(ITelegramBotClient botClient, Message message)
        {
            var words = message.Text.Split(new char[] { '?', '@' }, StringSplitOptions.RemoveEmptyEntries);
            string text;
            if (words.Length != 2)
            {
                text = "Введите свойство по которому будет происходить фильтрация: /filtered_events?(property)";
            }
            else
            {
                var events = await _googleCalendar.GetEvents(q: words[1]);
                text = await _googleCalendar.ShowUpCommingEvents(events);
            }
            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                        text: text);
        }
    }
}
