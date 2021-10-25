using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TelegramBotService;

namespace TelegramBotBusiness.Services
{
    public class ConfigureWebhookService : IHostedService
    {
        private readonly ILogger<ConfigureWebhookService> _logger;
        private readonly IServiceProvider _service;
        private AbstractTelegramBot _bot;

        public ConfigureWebhookService(
            ILogger<ConfigureWebhookService> logger,
            IServiceProvider service)
        {
            _logger = logger;
            _service = service;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var scope = _service.CreateScope();
                _bot = scope.ServiceProvider.GetRequiredService<AbstractTelegramBot>();
                await _bot.StartAsync((int)Mode.Updates);
                _logger.LogInformation("Webhook started!");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                await Task.FromCanceled(cancellationToken);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Webhook stoped!");
            await AbstractTelegramBot.StopAsync();
        }
    }
}
