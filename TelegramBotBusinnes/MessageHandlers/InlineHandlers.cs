using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotBusiness.MessageHandlers
{
    public class InlineHandlers
    {
        public static async Task<Message> SendInlineKeyboard(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Вывести сообщение", "Выведенное сообщение"),
                        InlineKeyboardButton.WithCallbackData("Все события календаря", "/all_events"),
                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Отфильтрованные события", "/filtered_events_query"),
                        InlineKeyboardButton.WithCallbackData("2.2", "22"),
                    },
                });

            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                        text: "Choose",
                                                        replyMarkup: inlineKeyboard);
        }
    }
}
