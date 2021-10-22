using GoogleCalendarService;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotBusiness.MessageHandlers
{
    public class CalendarMessagesHandlers
    {
        private readonly IGoogleCalendar _googleCalendar;

        public CalendarMessagesHandlers(IGoogleCalendar googleCalendar)
        {
            _googleCalendar = googleCalendar;
        }

        public async Task<Message> SendFilteredCalendarEvents(ITelegramBotClient botClient, Message message)
        {
            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                        text: await _googleCalendar.FilteredEventsInlineCommandHandler(message.Text),
                                                        replyMarkup: new ReplyKeyboardRemove());
        }

        public async Task<Message> SendAllCalendarEvents(ITelegramBotClient botClient, Message message)
        {
            var text = await _googleCalendar.ShowUpCommingEvents();

            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                        text: text,
                                                        replyMarkup: new ReplyKeyboardRemove());
        }

        public async Task<Message> SendEventsInTimeInterval(ITelegramBotClient botClient, Message message)
        {
            try
            {
                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: await _googleCalendar.DayEventsInTimeIntervalCommandHandler(message.Text),
                                                            replyMarkup: new ReplyKeyboardRemove());
            }
            catch
            {
                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                               text: "Input error",
                                                               replyMarkup: new ReplyKeyboardRemove());
            }
        }

        public async Task<Message> SendEventsInDateTimeInterval(ITelegramBotClient botClient, Message message)
        {
            try
            {
                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                            text: await _googleCalendar.EventsInDateTimeIntervalCommandHandler(message.Text),
                                                            replyMarkup: new ReplyKeyboardRemove());
            }
            catch
            {
                return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                               text: "Input error",
                                                               replyMarkup: new ReplyKeyboardRemove());
            }
        }
    }
}
