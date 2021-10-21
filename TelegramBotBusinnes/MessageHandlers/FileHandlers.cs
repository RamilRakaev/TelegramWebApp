using System;
using System.IO;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace TelegramBotBusiness.MessageHandlers
{
    public class FileHandlers
    {
        public static async Task<Message> SendFile(ITelegramBotClient botClient, Message message)
        {
            await botClient.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

            string filePath = $"{Environment.CurrentDirectory}\\wwwroot\\img\\Squidward I'm hot.png";
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var fileName = filePath.Split(Path.DirectorySeparatorChar).Last();
            
            return await botClient.SendPhotoAsync(chatId: message.Chat.Id,
                                                  photo: new InputOnlineFile(fileStream, fileName),
                                                  caption: "OH God, I'm Hot!");
        }
    }
}
