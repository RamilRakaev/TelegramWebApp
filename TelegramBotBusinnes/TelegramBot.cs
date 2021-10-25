using Domain.Interfaces;
using Domain.Model;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types.Enums;
using TelegramBotService;

namespace TelegramBotBusiness
{
    public class TelegramBot : AbstractTelegramBot
    {
        
        public TelegramBot(AbstractTelegramHandlers handlers, IRepository<Option> repository) : 
            base(handlers)
        {
            ConfigureTelegramBot(repository.GetAllAsNoTracking().ToArray());
        }

        public static BotStatus BotStatus { get; protected set; } = BotStatus.Off;
        
        public override Task ConfigureTelegramBot(Option[] options)
        {
            _options = options.ToDictionary(o => o.PropertyName, o => o.Value);
            return Task.CompletedTask;
        }

        public override async Task StartAsync(int mode)
        {
            if(mode == (int)Mode.Updates)
            {
                await StartReceiving();
                BotStatus = BotStatus.OnInUpdatesMode;
            }
            else if(mode == (int)Mode.Webhook)
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
            if (_options["HostAddress"] != null)
            {
                await Bot.SetWebhookAsync(
                    url: _options["HostAddress"],
                    allowedUpdates: Array.Empty<UpdateType>(),
                    cancellationToken: Cts.Token);
            }
            else
            {
                throw new NullReferenceException("HostAddress is null");
            }
        }

        public new static async Task  StopAsync()
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
