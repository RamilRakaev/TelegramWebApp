using Domain.Interfaces;
using Domain.Model;
using GoogleCalendarBusiness;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotBusiness.CallbackQueriesHandlers;
using TelegramBotBusiness.MessageHandlers;
using TelegramBotService;

namespace TelegramBotBusiness
{
    public class HandlerConfiguration : ITelegramConfiguration
    {
        private readonly OutputCallbackQueryHandler queryHandler;
        private readonly CalendarHandlers calendarHandlers;

        public HandlerConfiguration(IRepository<Option> optionRepository)
        {
            var options = optionRepository.GetAllAsNoTracking().ToArray();
            var googleCalendar = new GoogleCalendar(options);
            queryHandler = new OutputCallbackQueryHandler(googleCalendar);
            calendarHandlers = new CalendarHandlers(googleCalendar);
        }

        public void Configurate(AbstractTelegramHandlers handlers)
        {
            handlers.TextMessageHandlers = new List<TextMessageHandler>()
            {
                new TextMessageHandler("/inline_mode",
                "send inline keyboard",
                (ITelegramBotClient botClient, Message message) => InlineHandlers.SendInlineKeyboard(botClient, message)),

                new TextMessageHandler("/photo",
                "send a photo",
                (ITelegramBotClient botClient, Message message) => FileHandlers.SendFile(botClient, message)),

                new TextMessageHandler("/request",
                "request location or contact",
                (ITelegramBotClient botClient, Message message) => RequestHandlers.RequestContactAndLocation(botClient, message)),

                new TextMessageHandler("/filtered_events",
                "calendar events filtered by property",
                (ITelegramBotClient botClient, Message message) => calendarHandlers.SendFilteredCalendarEvents(botClient, message)),

                new TextMessageHandler("/all_events",
                "all calendar events",
                (ITelegramBotClient botClient, Message message) => calendarHandlers.SendAllCalendarEvents(botClient, message))
            };

            handlers.CallbackQueryHandlers = new List<CallbackQueryMessageHandler>()
            {
                new CallbackQueryMessageHandler(
                    "/all_events",
                    queryHandler.BotOnGetAllEventsReceived),
                new CallbackQueryMessageHandler(
                    "/filtered_events_query",
                    queryHandler.BotOnGetFilteredEventsReceived)
            };
        }
    }
}
