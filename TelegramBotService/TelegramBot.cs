using Domain.Model;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

namespace TelegramBotService
{
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

        public async Task Start()
        {
            _bot ??= new TelegramBotClient(_token);

            var me = await _bot.GetMeAsync();

            cts ??= new CancellationTokenSource();

            _bot.StartReceiving(
                new DefaultUpdateHandler(
                _handlers.HandleUpdateAsync,
                _handlers.HandleErrorAsync),
                               cts.Token);
        }

        public static void Stop()
        {
            if(cts != null) cts.Cancel();
        }
    }
}
