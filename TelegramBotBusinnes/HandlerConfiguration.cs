using GoogleCalendarService;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotBusiness.CallbackQueriesHandlers;
using TelegramBotBusiness.InlineQueriesHandlers;
using TelegramBotBusiness.MessageHandlers;
using TelegramBotService;

namespace TelegramBotBusiness
{
    public class HandlerConfiguration : ITelegramConfiguration
    {
        private readonly OutputCallbackQueryHandler queryHandler;
        private readonly CalendarMessagesHandlers calendarMessagesHandlers;
        private readonly CalendarInlineQueriesHandlers calendarInlineQueriesHandlers;

        public HandlerConfiguration(IGoogleCalendar googleCalendar)
        {
            queryHandler = new OutputCallbackQueryHandler(googleCalendar);
            calendarMessagesHandlers = new CalendarMessagesHandlers(googleCalendar);
            calendarInlineQueriesHandlers = new CalendarInlineQueriesHandlers(googleCalendar);
        }

        public void Configurate(AbstractTelegramHandlers handlers)
        {
            handlers.textMessageCommandHandlers = new List<TextMessageCommandHandler>()
            {
                new TextMessageCommandHandler("/inline_mode",
                "send inline keyboard",
                (ITelegramBotClient botClient, Message message) => InlineHandlers.SendInlineKeyboard(botClient, message)),

                new TextMessageCommandHandler("/photo",
                "send a photo",
                (ITelegramBotClient botClient, Message message) => FileHandlers.SendFile(botClient, message)),

                new TextMessageCommandHandler("/request",
                "request location or contact",
                (ITelegramBotClient botClient, Message message) => RequestHandlers.RequestContactAndLocation(botClient, message)),

                new TextMessageCommandHandler("/all_events",
                "all calendar events",
                (ITelegramBotClient botClient, Message message) => calendarMessagesHandlers.SendAllCalendarEvents(botClient, message)),

                new TextMessageCommandHandler("/filtered_events",
                "calendar events filtered by property for example: filtered_events?(property)",
                (ITelegramBotClient botClient, Message message) => calendarMessagesHandlers.SendFilteredCalendarEvents(botClient, message)),

                new TextMessageCommandHandler("/time_interval",
                "today's events in specific time interval for example: time_interval?10:00-20:00",
                (ITelegramBotClient botClient, Message message) => calendarMessagesHandlers.SendEventsInTimeInterval(botClient, message))
            };

            handlers.callbackQueryCommandHandlers = new List<CallbackQueryCommandHandler>()
            {
                new CallbackQueryCommandHandler(
                    "/all_events",
                    queryHandler.BotOnGetAllEventsReceived),
                new CallbackQueryCommandHandler(
                    "/filtered_events_query",
                    queryHandler.BotOnGetFilteredEventsReceived),
                new CallbackQueryCommandHandler(
                    "/time_interval_query",
                    queryHandler.BotOnGetEventsInTimeIntervalReceived)
            };

            handlers.inlineQueryCommandHandlers = new List<InlineQueryCommandHandler>()
            {
                new InlineQueryCommandHandler("filtered events",
                calendarInlineQueriesHandlers.BotOnGetFilteredEventsQueryReceived
                ),
                new InlineQueryCommandHandler("all events",
                calendarInlineQueriesHandlers.BotOnGetAllEventsQueryReceived
                ),
                new InlineQueryCommandHandler("time interval",
                calendarInlineQueriesHandlers.BotOnGetEventsInTimeIntervalQueryReceived
                )
            };
        }
    }
}
