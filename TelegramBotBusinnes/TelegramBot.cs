using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

namespace TelegramBotService
{
    //public class TelegramBot
    //{
    //    private TelegramBotClient _bot;
    //    private CancellationTokenSource cts;
    //    private readonly string _token;
    //    private readonly ITelegramHandlers _handlers;

    //    public TelegramBot(string token, ITelegramHandlers handlers)
    //    {
    //        _token = token;
    //        _handlers = handlers;
    //    }

    //    public async Task Start()
    //    {
    //        _bot = new TelegramBotClient(_token);

    //        var me = await _bot.GetMeAsync();

    //        cts = new CancellationTokenSource();

    //        // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
    //        _bot.StartReceiving(
    //            new DefaultUpdateHandler(
    //            _handlers.HandleUpdateAsync, 
    //            _handlers.HandleErrorAsync),
    //                           cts.Token);
    //    }

    //    public void Stop()
    //    {
    //        cts.Cancel();
    //    }
    //}
}
