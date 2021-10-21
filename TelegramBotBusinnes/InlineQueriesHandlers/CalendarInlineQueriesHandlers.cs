using GoogleCalendarService;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;
using TelegramBotService;

namespace TelegramBotBusiness.InlineQueriesHandlers
{
    public class CalendarInlineQueriesHandlers
    {
        private readonly IGoogleCalendar _googleCalendar;

        public CalendarInlineQueriesHandlers(IGoogleCalendar googleCalendar)
        {
            _googleCalendar = googleCalendar;
        }

        public async Task<InlineQueryHandler> BotOnGetAllEventsQueryReceived(ITelegramBotClient botClient, InlineQuery inlineQuery)
        {
            InlineQueryResultBase[] results = {
                new InlineQueryResultArticle(
                    "2",
                    "all events",
                    new InputTextMessageContent(
                        await _googleCalendar.ShowUpCommingEvents()
                    )
                )
            };
            await botClient.AnswerInlineQueryAsync(
                inlineQueryId: inlineQuery.Id,
                results: results,
                isPersonal: true,
                cacheTime: 0);
            return null;
        }

        public async Task BotOnGetFilteredEventsQueryReceived(ITelegramBotClient botClient, InlineQuery inlineQuery)
        {
            try
            {
                var property = inlineQuery.Query.Split(' ').Last();
                var events = await _googleCalendar.GetEvents(q: property);
                var textMessage = await _googleCalendar.ShowUpCommingEvents(events);

                InlineQueryResultBase[] results = {
                new InlineQueryResultArticle(
                    id: "3",
                    title: "filtered events",
                    new InputTextMessageContent(textMessage)
                    )
                };

                await botClient.AnswerInlineQueryAsync(
                    inlineQueryId: inlineQuery.Id,
                    results: results,
                    isPersonal: true,
                    cacheTime: 0);
            }
            catch
            { }
        }

        public async Task<Message> WaitForThePropertyToBeEntered(ITelegramBotClient botClient, Message message)
        {
            var events = await _googleCalendar.GetEvents(q: message.Text);
            var textMessage = await _googleCalendar.ShowUpCommingEvents(events);
            return await botClient.SendTextMessageAsync(message.Chat.Id, textMessage);
        }

        public async Task BotOnGetEventsInTimeIntervalQueryReceived(ITelegramBotClient botClient, InlineQuery inlineQuery)
        {
            try
            {
                var text = inlineQuery.Query.Split(' ').Last();
                int startHours = Convert.ToInt32(text.Substring(0, 2));
                int startMinutes = Convert.ToInt32(text.Substring(3, 2));
                int endHours = Convert.ToInt32(text.Substring(6, 2));
                int endMinutes = Convert.ToInt32(text.Substring(9, 2));
                var textMessage = await _googleCalendar.ShowDayEventsInTimeInterval(startHours, startMinutes, endHours, endMinutes);

                InlineQueryResultBase[] results = {
                new InlineQueryResultArticle(
                    id: "4",
                    title: "events in interval",
                    new InputTextMessageContent(textMessage)
                    )
                };

                await botClient.AnswerInlineQueryAsync(
                    inlineQueryId: inlineQuery.Id,
                    results: results,
                    isPersonal: true,
                    cacheTime: 0);
            }
            catch
            { }
        }
    }
}
