using Domain.Model;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotService
{
    public enum Mode { Updates, Webhook }
    public enum BotStatus { OnInUpdatesMode, OnInWebhookMode, Off }
    
    public abstract class AbstractTelegramBot
    {
        private static string _token;

        public AbstractTelegramBot(AbstractTelegramHandlers handlers)
        {
            Handlers = handlers;
        }

        protected static CancellationTokenSource Cts { get; private set; }
        protected static AbstractTelegramHandlers Handlers { get; private set; }
        protected static TelegramBotClient Bot { get; private set; }

        public virtual Task ConfigureTelegramBot(TelegramOptions options)
        {
            _token = options.Token;
            return Task.CompletedTask;
        }

        protected static void CreateBot()
        {
            if (_token != null)
            {
                Bot = new TelegramBotClient(_token);
                Cts = new CancellationTokenSource();
            }
            else
            {
                throw new NullReferenceException("Token is null");
            }
        }

        public static async Task EchoAsync(Update update)
        {
            if (Bot != null)
            {
                await Handlers.HandleUpdateAsync(Bot, update, Cts.Token);
            }
        }

        public abstract Task StartAsync(Mode mode);

        public abstract Task StopAsync();

        public static bool IsIncluded()
        {
            if (Bot == null)
            {
                return false;
            }
            return true;
        }
    }
}
