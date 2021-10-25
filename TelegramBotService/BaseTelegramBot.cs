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
    public enum Mode { Updates, Webhook }
    public enum BotStatus { OnInUpdatesMode, OnInWebhookMode, Off }

    public class BaseTelegramBot
    {
        private static Dictionary<string, string> _options;
        private static TelegramBotClient _bot;
        private static CancellationTokenSource _cts;
        private static AbstractTelegramHandlers _handlers;

        public BaseTelegramBot(AbstractTelegramHandlers handlers, Option[] options)
        {
            _handlers = handlers;
            _options = options.ToDictionary(o => o.PropertyName, o => o.Value);
        }

        public static BotStatus BotStatus { get; protected set; } = BotStatus.Off;

        public async Task StartAsync(Mode mode)
        {
            await StopAsync();
            CreateBot();
            if (mode == Mode.Updates)
            {
                await StartReceiving();
                BotStatus = BotStatus.OnInUpdatesMode;
            }
            else if (mode == Mode.Webhook)
            {
                await StartInterception();
                BotStatus = BotStatus.OnInWebhookMode;
            }
        }

        private Task StartReceiving()
        {
            _bot.StartReceiving(
                new DefaultUpdateHandler(
                _handlers.HandleUpdateAsync,
                _handlers.HandleErrorAsync),
                               _cts.Token);
            return Task.CompletedTask;
        }

        private async Task StartInterception()
        {
            if (_options["HostAddress"] != null)
            {
                await _bot.SetWebhookAsync(
                    url: _options["HostAddress"],
                    allowedUpdates: Array.Empty<UpdateType>(),
                    cancellationToken: _cts.Token);
            }
            else
            {
                throw new NullReferenceException("HostAddress is null");
            }
        }

        public static async Task StopAsync()
        {
            if (BotStatus == BotStatus.OnInUpdatesMode)
            {
                if (_cts != null) { _cts.Cancel(); }
            }
            else if (BotStatus == BotStatus.OnInWebhookMode)
            {
                if (_bot != null)
                {
                    await _bot.DeleteWebhookAsync(cancellationToken: _cts.Token);
                }
            }
        }
        
        protected static void CreateBot()
        {
            if (_options["BotToken"] != null)
            {
                _bot = new TelegramBotClient(_options["BotToken"]);
                _cts = new CancellationTokenSource();
            }
            else
            {
                throw new NullReferenceException("Token is null");
            }
        }

        public static async Task EchoAsync(Update update)
        {
            if (_bot != null)
            {
                await _handlers.HandleUpdateAsync(_bot, update, _cts.Token);
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
