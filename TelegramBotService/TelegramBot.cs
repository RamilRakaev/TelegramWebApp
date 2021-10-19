using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBotService
{
    public enum BotStatus { On, Off }

    public class TelegramBot
    {
        private static TelegramBotClient _bot;
        private static CancellationTokenSource cts;
        private static string _token;
        private static AbstractTelegramHandlers _handlers;
        private static Dictionary<string, string> _options;

        public TelegramBot(Option[] options, AbstractTelegramHandlers handlers)
        {
            _options = options.ToDictionary(o => o.PropertyName, o => o.Value);
            _token = _options["Token"];
            if (_token == null)
                throw new ArgumentNullException("Token is null");
            _handlers = handlers;
        }

        public static async Task EchoAsync(Update update)
        {
            if (_bot != null)
            {
                await _handlers.HandleUpdateAsync(_bot, update, cts.Token);
            }
        }

        public async Task StartReceiving()
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

        public static void Stop()
        {
            if (cts != null) { cts.Cancel(); }
        }

        public async Task StartInterception()
        {
            _bot = new TelegramBotClient(_token);
            cts = new CancellationTokenSource();
            await _bot.SetWebhookAsync(
                url: _options["Webhook"],
                allowedUpdates: Array.Empty<UpdateType>(),
                cancellationToken: cts.Token);
        }

        public static async Task StopInterception()
        {
            if (_bot != null)
            {
                await _bot.DeleteWebhookAsync(cancellationToken: cts.Token);
            }
        }

        public static bool IsIncluded()
        {
            if (_bot == null)
            {
                return false;
            }
            return true;
        }
    }
}
