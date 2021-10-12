using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using GoogleCalendarService;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleCalendarBusiness
{
    public class GoogleCalendar : IGoogleCalendar
    {
        public static string[] Scopes;

        private readonly GoogleCalendarOptions _options;

        static GoogleCalendar()
        {
            Scopes = new string[] { CalendarService.Scope.Calendar };
        }

        public GoogleCalendar(IOptions<GoogleCalendarOptions> options)
        {
            _options = options.Value;
        }

        public async Task<string> ShowUpCommingEvents(Event[] events = null)
        {
            string output = "";
            events = events ?? await GetEvents();
            if (events.Length > 0)
            {
                foreach (var eventItem in events)
                {
                    var description = eventItem.Description != null ? eventItem.Description : "Описание отсутствует";
                    var start = eventItem.Start.DateTime != null ? eventItem.Start.DateTime.Value.ToString("g") : "";
                    var end = eventItem.End.DateTime != null ? eventItem.End.DateTime.Value.ToString("g") : "";
                    output += string
                        .Concat($"{eventItem.Summary} " +
                        $"({start} - " +
                        $"{end}): \n" +
                        $"{description}\n");
                }
            }
            else
            {
                output = "Нет запланированных событий";
            }
            return output;
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

            var request = service.Events.List(_options.CalendarId);
            request.Fields = "items(summary,description,start,end)";
            request.TimeMin = timeMin;
            request.TimeMax = timeMax;
            request.ShowDeleted = showDeleted;
            request.SingleEvents = singleEvents;
            request.ShowHiddenInvitations = showHiddenInvitations;
            request.MaxResults = maxResults;
            request.OrderBy = sortByModifiedDate ? EventsResource.ListRequest.OrderByEnum.Updated : EventsResource.ListRequest.OrderByEnum.StartTime;
            request.Q = q;
            Events events = await request.ExecuteAsync();
            return events.Items != null && events.Items.Count > 0 ? events.Items.ToArray() : new Event[0];
        }

        private CalendarService GetService()
        {
            BaseClientService.Initializer initializer = new BaseClientService.Initializer
            {
                ApiKey = _options.ApiKey,
                ApplicationName = _options.ApplicationName
            };
            return new CalendarService(initializer);
        }
    }
}
