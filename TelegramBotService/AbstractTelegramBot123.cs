using Domain.Model;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramBotService
{
    //public abstract class AbstractTelegramBot123
    //{
    //    private static TelegramBotClient _bot;
    //    private static CancellationTokenSource cts;
    //    private static AbstractTelegramHandlers _handlers;
    //    protected static string _hostAddress;
    //    protected static string _token;

    //    public AbstractTelegramBot123(AbstractTelegramHandlers handlers)
    //    {
    //        _handlers = handlers;
    //    }

    //    public static Task ConfigureTelegramBot(TelegramOptions options)
    //    {
    //        _hostAddress = options.HostAddress;
    //        _token = options.Token;
    //        return Task.CompletedTask;
    //    }

    //    public static async Task EchoAsync(Update update)
    //    {
    //        if (_bot != null)
    //        {
    //            await _handlers.HandleUpdateAsync(_bot, update, cts.Token);
    //        }
    //    }

    //    public Task StartReceiving()
    //    {
    //        StopReceiving();

    //        CreateBot();
    //        _bot.StartReceiving(
    //            new DefaultUpdateHandler(
    //            _handlers.HandleUpdateAsync,
    //            _handlers.HandleErrorAsync),
    //                           cts.Token);

    //        return Task.CompletedTask;
    //    }

    //    public static void StopReceiving()
    //    {
    //        if (cts != null) { cts.Cancel(); }
    //    }

    //    public async Task StartInterception()
    //    {
    //        await StopInterception();
    //        CreateBot();
    //        if (_hostAddress != null)
    //        {
    //            await _bot.SetWebhookAsync(
    //                url: _hostAddress,
    //                allowedUpdates: Array.Empty<UpdateType>(),
    //                cancellationToken: cts.Token);
    //        }
    //        else
    //        {
    //            throw new NullReferenceException("HostAddress is null");
    //        }
    //    }

    //    public static async Task StopInterception()
    //    {
    //        if (_bot != null)
    //        {
    //            await _bot.DeleteWebhookAsync(cancellationToken: cts.Token);
    //        }
    //    }

    //    public static bool IsIncluded()
    //    {
    //        if (_bot == null)
    //        {
    //            return false;
    //        }
    //        return true;
    //    }

    //    private static void CreateBot()
    //    {
    //        if (_token != null)
    //        {
    //            _bot = new TelegramBotClient(_token);
    //            cts = new CancellationTokenSource();
    //        }
    //        else
    //        {
    //            throw new NullReferenceException("Token is null");
    //        }
    //    }
    //}
}
