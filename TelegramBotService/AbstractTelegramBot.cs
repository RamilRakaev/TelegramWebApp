using Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotService
{
    public enum Mode : int { Updates, Webhook }
    public enum BotStatus { OnInUpdatesMode, OnInWebhookMode, Off }
    
    public abstract class AbstractTelegramBot
    {
        protected static Dictionary<string, string> _options;
        public AbstractTelegramBot(AbstractTelegramHandlers handlers)
        {
            Handlers = handlers;
        }

        protected static CancellationTokenSource Cts { get; private set; }
        protected static AbstractTelegramHandlers Handlers { get; private set; }
        protected static TelegramBotClient Bot { get; private set; }

        public abstract Task ConfigureTelegramBot(Option[] options);
        

        protected static void CreateBot()
        {
            if (_options["Token"] != null)
            {
                Bot = new TelegramBotClient(_options["Token"]);
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

        public abstract Task StartAsync(int mode);

        public static Task StopAsync() { throw new Exception(); }

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
