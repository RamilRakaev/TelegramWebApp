using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class MigrationManager : IHostedService, IDisposable
    {
        private readonly IHost _host;
        private readonly ILogger<MigrationManager> _logger;
        private Timer _timer;

        public MigrationManager(IHost host,
            ILogger<MigrationManager> logger)
        {
            _host = host;
            _logger = logger;
        }

        private void MigrateDatabase(object state)
        {
            using var scope = _host.Services.CreateScope();
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
            _timer = new Timer(MigrateDatabase, null, TimeSpan.Zero, TimeSpan.FromHours(5));
            _logger.LogInformation($"Start timer of MigragionManager: {DateTime.Now:T}");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            _logger.LogInformation($"Stop timer of MigragionManager: {DateTime.Now:T}");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
