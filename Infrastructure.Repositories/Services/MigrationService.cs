using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services.Repositories
{
    public class MigrationService : IHostedService
    {
        private readonly IServiceProvider _service;
        private readonly ILogger<MigrationService> _logger;

        public MigrationService(IServiceProvider service,
            ILogger<MigrationService> logger)
        {
            _service = service;
            _logger = logger;
        }

        private void MigrateDatabase()
        {
            using var scope = _service.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            try
            {
                appContext.Database.Migrate();
                _logger.LogInformation($"Migrate: {DateTime.Now:T}");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start MigrationService: {DateTime.Now:T}");
            MigrateDatabase();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stop MigrationService: {DateTime.Now:T}");
            return Task.CompletedTask;
        }
    }
}
