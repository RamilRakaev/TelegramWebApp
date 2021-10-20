using Domain.Interfaces;
using Domain.Model;
using GoogleCalendarBusiness;
using System.Collections.Generic;
using System.Linq;
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

        public HandlerConfiguration(IRepository<Option> optionRepository)
        {
            var options = optionRepository.GetAllAsNoTracking().ToArray();
            var googleCalendar = new GoogleCalendar(options);
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

                new TextMessageCommandHandler("/filtered_events",
                "calendar events filtered by property",
                (ITelegramBotClient botClient, Message message) => calendarMessagesHandlers.SendFilteredCalendarEvents(botClient, message)),

                new TextMessageCommandHandler("/all_events",
                "all calendar events",
                (ITelegramBotClient botClient, Message message) => calendarMessagesHandlers.SendAllCalendarEvents(botClient, message))
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
                    "/events_in_interval_query",
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
