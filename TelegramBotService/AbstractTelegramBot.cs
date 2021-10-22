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

    public abstract class AbstractTelegramBot
    {
        private static TelegramBotClient _bot;
        private static CancellationTokenSource cts;
        private static string _token;
        private static AbstractTelegramHandlers _handlers;
        private readonly Dictionary<string, string> _options;

        public AbstractTelegramBot(Option[] options, AbstractTelegramHandlers handlers)
        {
            _options = options.ToDictionary(o => o.PropertyName, o => o.Value);
            if (_options.ContainsKey("Token"))
            {
                _token = _options["Token"];
            }
            else
            {
                throw new ArgumentNullException("Token is null");
            }
            _handlers = handlers;
        }

        public Task ConfigureWebhook(string Webhook)
        {
            _options.Add("Webhook", Webhook);
            return Task.CompletedTask;
        }

        public static async Task EchoAsync(Update update)
        {
            if (_bot != null)
            {
                await _handlers.HandleUpdateAsync(_bot, update, cts.Token);
            }
        }

        public Task StartReceiving()
        {
            StopReceiving();

            _bot = new TelegramBotClient(_token);

            cts = new CancellationTokenSource();

            _bot.StartReceiving(
                new DefaultUpdateHandler(
                _handlers.HandleUpdateAsync,
                _handlers.HandleErrorAsync),
                               cts.Token);

            return Task.CompletedTask;
        }

        public static void StopReceiving()
        {
            if (cts != null) { cts.Cancel(); }
        }

        public async Task StartInterception()
        {
            await StopInterception();

            _bot = new TelegramBotClient(_token);
            cts = new CancellationTokenSource();
            if (_options.ContainsKey("Webhook"))
            {
                await _bot.SetWebhookAsync(
                    url: _options["Webhook"],
                    allowedUpdates: Array.Empty<UpdateType>(),
                    cancellationToken: cts.Token);
            }
            else
            {
                throw new NullReferenceException("Webhook is null");
            }
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
