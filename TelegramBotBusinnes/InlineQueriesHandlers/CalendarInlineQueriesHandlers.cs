using GoogleCalendarService;
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
                InlineQueryResultBase[] results = {
                new InlineQueryResultArticle(
                    id: "3",
                    title: "filtered events",
                    new InputTextMessageContent(await _googleCalendar.FilteredEventsInlineQueryHandler(inlineQuery.Query))
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

        public async Task BotOnGetEventsInTimeIntervalQueryReceived(ITelegramBotClient botClient, InlineQuery inlineQuery)
        {
            try
            {
                InlineQueryResultBase[] results = {
                new InlineQueryResultArticle(
                    id: "4",
                    title: "events in time interval",
                    new InputTextMessageContent(await _googleCalendar.DayEventsInTimeIntervalQueryHandler(inlineQuery.Query))
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

        public async Task BotOnGetEventsInDateTimeIntervalQueryReceived(ITelegramBotClient botClient, InlineQuery inlineQuery)
        {
            try
            {
                InlineQueryResultBase[] results = {
                new InlineQueryResultArticle(
                    id: "5",
                    title: "events in  datetime interval",
                    new InputTextMessageContent(await _googleCalendar.EventsInDateTimeIntervalQueryHandler(inlineQuery.Query))
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
