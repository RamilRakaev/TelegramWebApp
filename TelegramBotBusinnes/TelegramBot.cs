using Domain.Model;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types.Enums;
using TelegramBotService;

namespace TelegramBotBusiness
{

    public class TelegramBot : AbstractTelegramBot
    {
        public TelegramBot(AbstractTelegramHandlers handlers, IOptions<TelegramOptions> options) : 
            base(handlers)
        {
            ConfigureTelegramBot(options.Value);
        }

        public static BotStatus BotStatus { get; protected set; } = BotStatus.Off;
        protected static string HostAddress { get; private set; }

        public override Task ConfigureTelegramBot(TelegramOptions options)
        {
            HostAddress = options.HostAddress;
            return ConfigureTelegramBot(options);
        }

        public override async Task StartAsync(Mode mode)
        {
            if(mode == Mode.Updates)
            {
                await StartReceiving();
                BotStatus = BotStatus.OnInUpdatesMode;
            }
            else if(mode == Mode.Webhook)
            {
                await StartInterception();
                BotStatus = BotStatus.OnInWebhookMode;
            }
        }

        private async Task StartReceiving()
        {
            await StopAsync();
            CreateBot();
            Bot.StartReceiving(
                new DefaultUpdateHandler(
                Handlers.HandleUpdateAsync,
                Handlers.HandleErrorAsync),
                               Cts.Token);
        }

        private async Task StartInterception()
        {
            await StopAsync();
            CreateBot();
            if (HostAddress != null)
            {
                await Bot.SetWebhookAsync(
                    url: HostAddress,
                    allowedUpdates: Array.Empty<UpdateType>(),
                    cancellationToken: Cts.Token);
            }
            else
            {
                throw new NullReferenceException("HostAddress is null");
            }
        }

        public override async Task StopAsync()
        {
            if (BotStatus == BotStatus.OnInUpdatesMode)
            {
                if (Cts != null) { Cts.Cancel(); }
            }
            else if (BotStatus == BotStatus.OnInWebhookMode)
            {
                if (Bot != null)
                {
                    await Bot.DeleteWebhookAsync(cancellationToken: Cts.Token);
                }
            }
        }
    }
}
