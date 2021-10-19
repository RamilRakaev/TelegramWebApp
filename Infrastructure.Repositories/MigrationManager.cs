using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class MigrationManager : IHostedService
    {
        private readonly IServiceProvider _service;
        private readonly ILogger<MigrationManager> _logger;

        public MigrationManager(IServiceProvider service,
            ILogger<MigrationManager> logger)
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start timer of MigragionManager: {DateTime.Now:T}");
            MigrateDatabase();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stop timer of MigragionManager: {DateTime.Now:T}");
            return Task.CompletedTask;
        }
    }
}
