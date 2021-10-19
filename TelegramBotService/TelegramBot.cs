using Domain.Model;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace TelegramBotService
{
    public enum BotStatus{ On, Off }

    public class TelegramBot
    {
        private static TelegramBotClient _bot;
        private static CancellationTokenSource cts;
        private static string _token;
        private static AbstractTelegramHandlers _handlers;

        public TelegramBot(Option[] options, AbstractTelegramHandlers handlers)
        {
            _token = options.FirstOrDefault(o => o.PropertyName == "Token").Value;
            if (_token == null)
                throw new ArgumentNullException("Token is null");
            _handlers = handlers;
        }

        public static async Task EchoAsync(Update update)
        {
            if(_bot != null)
            {
                await _handlers.HandleUpdateAsync(_bot, update, cts.Token);
            }
        }

        public static async Task Start()
        {
            _bot = new TelegramBotClient(_token);

            var me = await _bot.GetMeAsync();

            cts = new CancellationTokenSource();

            _bot.StartReceiving(
                new DefaultUpdateHandler(
                _handlers.HandleUpdateAsync,
                _handlers.HandleErrorAsync),
                               cts.Token);
        }

        public static bool IsIncluded()
        {
            if (_bot == null) 
            {
                return false;
            }
            return true;
        }

        public static void Stop()
        {
            if(cts != null) cts.Cancel();
        }
    }
}
