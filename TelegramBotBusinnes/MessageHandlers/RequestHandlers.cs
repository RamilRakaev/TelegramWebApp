using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotBusiness.MessageHandlers
{
    public class RequestHandlers
    {
        public static async Task<Message> RequestContactAndLocation(ITelegramBotClient botClient, Message message)
        {
            var RequestReplyKeyboard = new ReplyKeyboardMarkup(new[]
            {
                    KeyboardButton.WithRequestLocation("Location"),
                    KeyboardButton.WithRequestContact("Contact"),
                    KeyboardButton.WithRequestPoll("Poll"),
                });

            return await botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                        text: "Who or Where are you?",
                                                        replyMarkup: RequestReplyKeyboard);
        }
    }
}
