using GoogleCalendarService;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotBusiness.CallbackQueryHandlers;
using TelegramBotBusiness.MessageHandlers;
using TelegramBotService;

namespace TelegramBotBusiness
{
    public class HandlerConfiguration : ITelegramConfiguration
    {
        OutputCallbackQueryHandler queryHandler;
        CalendarHandlers calendarHandlers;

        public HandlerConfiguration(IGoogleCalendar googleCalendar)
        {
            queryHandler = new OutputCallbackQueryHandler(googleCalendar);
            calendarHandlers = new CalendarHandlers(googleCalendar);
        }
        public void Configurate(ITelegramHandlers handlers)
        {
            handlers.TextMessageHandlers = new List<TextMessageHandler>()
            {
                new TextMessageHandler("/inline_mode",
                "send inline keyboard",
                (ITelegramBotClient botClient, Message message) => InlineHandlers.SendInlineKeyboard(botClient, message)),

                new TextMessageHandler("/keyboard",
                "send custom keyboard",
                (ITelegramBotClient botClient, Message message) => KeyboardHandlers.SendReplyKeyboard(botClient, message)),

                new TextMessageHandler("/remove_keyboard",
                "remove custom keyboard",
                (ITelegramBotClient botClient, Message message) => KeyboardHandlers.RemoveKeyboard(botClient, message)),

                new TextMessageHandler("/photo",
                "send a photo",
                (ITelegramBotClient botClient, Message message) => FileHandlers.SendFile(botClient, message)),

                new TextMessageHandler("/request",
                "request location or contact",
                (ITelegramBotClient botClient, Message message) => RequestHandlers.RequestContactAndLocation(botClient, message)),

                new TextMessageHandler("/filtered_events",
                "calendar events filtered by property",
                (ITelegramBotClient botClient, Message message) => calendarHandlers.SendFilteredCalendarEvents(botClient, message))
            };

            handlers.CallbackQueryHandlers = new List<CallbackQueryMessageHandler>()
            {
                new CallbackQueryMessageHandler(
                    "Выведенное сообщение",
                    queryHandler.BotOnCallbackQueryReceived),
                new CallbackQueryMessageHandler(
                    "/getallevents",
                    queryHandler.BotOnGetAllEventsReceived),
                new CallbackQueryMessageHandler(
                    "/filtered_events_query",
                    queryHandler.BotOnGetFilteredEventsReceived)
            };
        }
    }
}
