using Domain.Model;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using GoogleCalendarService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalendarBusiness
{
    public class GoogleCalendar : IGoogleCalendar
    {
        private readonly Dictionary<string, string> _options;

        static GoogleCalendar()
        {
            Scopes = new string[] { CalendarService.Scope.Calendar };
        }

        public GoogleCalendar(Option[] options)
        {
            _options = options.ToDictionary(o => o.PropertyName, o => o.Value);
        }

        public static string[] Scopes { get; private set; }

        public async Task<string> ShowDayEventsInTimeInterval(int startHours, int startMinutes, int endHours, int endMinutes)
        {
            var now = DateTime.Now;
            var timeMin = new DateTime(now.Year, now.Month, now.Day, startHours, startMinutes, 0);
            var timeMax = new DateTime(now.Year, now.Month, now.Day, endHours, endMinutes, 0);
            var events = await GetEvents(timeMin, timeMax);
            return await ShowUpCommingEvents(events);
        }

        public async Task<string> ShowUpCommingEvents(Event[] events = null)
        {
            var output = new StringBuilder("", 10000);
            events ??= await GetEvents();
            if (events.Length > 0)
            {
                foreach (var eventItem in events)
                {
                    var description = eventItem.Description ?? "Description is missing";
                    var start = eventItem.Start.DateTime != null ? eventItem.Start.DateTime.Value.ToString("g") : "";
                    var end = eventItem.End.DateTime != null ? eventItem.End.DateTime.Value.ToString("g") : "";
                    output.Append(
                        $"{eventItem.Summary}\n" +
                        $"({start} - {end}): \n" +
                        $"{description}\n" +
                        $"{eventItem.HtmlLink}\n\n");
                }
            }
            else
            {
                output.Clear();
                output.Append("No scheduled events");
            }
            return output.ToString();
        }

        public async Task<string> ShowFreeDays(int months)
        {
            var now = DateTime.Now;
            string freeDays = "";
            var currentDay = now;
            var events = await GetEvents();
            while(currentDay.Month < now.Month + months)
            {
                var calendarEvent = events.FirstOrDefault(e => 
                e.Start.DateTime.Value.Month == currentDay.Month &&
                currentDay.Day >= e.Start.DateTime.Value.Day &&
                currentDay.Day <= e.End.DateTime.Value.Day);

                if(calendarEvent == null)
                {
                    freeDays += currentDay.ToString("d") + "\n";
                }
                currentDay += new TimeSpan(1, 0, 0, 0);
            }
            return freeDays;
        }

        public async Task<Event[]> GetEvents(
            DateTime? timeMin = null,
            DateTime? timeMax = null,
            int maxResults = 250,
            bool showDeleted = false,
            bool singleEvents = true,
            bool showHiddenInvitations = false,
            string q = null,
            bool sortByModifiedDate = false)
        {
            CalendarService service = GetService();

            var request = service.Events.List(_options["CalendarId"]);
            request.Fields = "items(summary,description,start,end,htmlLink)";
            request.TimeMin = timeMin;
            request.TimeMax = timeMax;
            request.ShowDeleted = showDeleted;
            request.SingleEvents = singleEvents;
            request.ShowHiddenInvitations = showHiddenInvitations;
            request.MaxResults = maxResults;
            request.OrderBy = sortByModifiedDate ? EventsResource.ListRequest.OrderByEnum.Updated : EventsResource.ListRequest.OrderByEnum.StartTime;
            request.Q = q;
            Events events = await request.ExecuteAsync();
            return events.Items != null && events.Items.Count > 0 ? events.Items.ToArray() : Array.Empty<Event>();
        }

        private CalendarService GetService()
        {
            BaseClientService.Initializer initializer = new BaseClientService.Initializer
            {
                ApiKey = _options["ApiKey"]
            };
            return new CalendarService(initializer);
        }
    }
}
