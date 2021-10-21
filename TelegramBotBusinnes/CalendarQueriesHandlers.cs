using GoogleCalendarService;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TelegramBotBusiness
{
    public static class CalendarQueriesHandlers
    {
        public static async Task<string> FilteredEventsInlineQueryHandler(this IGoogleCalendar googleCalendar, string queryText)
        {
            var property = queryText.Split(' ').Last();
            return await googleCalendar.FilteredEvents(property);
        }

        public static async Task<string> FilteredEvents(this IGoogleCalendar googleCalendar, string property)
        {
            var events = await googleCalendar.GetEvents(q: property);
            var textMessage = await googleCalendar.ShowUpCommingEvents(events);
            return textMessage;
        }

        public static async Task<string> DayEventsInTimeInterval(this IGoogleCalendar googleCalendar, string text)
        {
            int startHours = Convert.ToInt32(text.Substring(0, 2));
            int startMinutes = Convert.ToInt32(text.Substring(3, 2));
            int endHours = Convert.ToInt32(text.Substring(6, 2));
            int endMinutes = Convert.ToInt32(text.Substring(9, 2));
            return await googleCalendar.ShowDayEventsInTimeInterval(startHours, startMinutes, endHours, endMinutes);
        }

        public static async Task<string> DayEventsInTimeIntervalQueryHandler(this IGoogleCalendar googleCalendar, string queryText)
        {
            var text = queryText.Split(' ').Last();
            return await googleCalendar.DayEventsInTimeInterval(text);
        }

        public static async Task<string> EventsInDateTimeIntervalCommandHandler(this IGoogleCalendar googleCalendar, string queryText)
        {
            var text = queryText[(queryText.IndexOf('?') + 1)..].Split('-');
            var startDateTime = Convert.ToDateTime(text[0]);
            var endDateTime = Convert.ToDateTime(text[1]);
            var events = await googleCalendar.GetEvents(startDateTime, endDateTime);
            var textMessage = await googleCalendar.ShowUpCommingEvents(events);
            return textMessage;
        }

        public static async Task<string> EventsInDateTimeInterval(this IGoogleCalendar googleCalendar, string queryText)
        {
            var text = queryText.Split("-");
            var startDateTime = Convert.ToDateTime(text[0]);
            var endDateTime = Convert.ToDateTime(text[1]);
            var events = await googleCalendar.GetEvents(startDateTime, endDateTime);
            var textMessage = await googleCalendar.ShowUpCommingEvents(events);
            return textMessage;
        }
    }
}
