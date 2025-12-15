using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace KineStat.Services
{
    /// <summary>
    /// Background service that automatically runs anonymization 
    /// patients every day at 2am
    /// </summary>
    public class PatientAnonymizationBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PatientAnonymizationBackgroundService> _logger;

        public PatientAnonymizationBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<PatientAnonymizationBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Service d'anonymisation RGPD démarré");

            await WaitForNextExecutionTime(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Exécution planifiée de l'anonymisation RGPD");

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var anonymizationService = scope.ServiceProvider
                            .GetRequiredService<PatientAnonymizationService>();

                        var anonymizedCount = await anonymizationService.AnonymizeExpiredPatientsAsync();

                        _logger.LogInformation(
                            $"Anonymisation terminée: {anonymizedCount} patients anonymisés");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERREUR lors de l'anonymisation planifiée");
                }

                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }

            _logger.LogInformation("Service d'anonymisation RGPD arrêté");
        }

        /// <summary>
        /// Calculate the delay until 2am
        /// </summary>
        private async Task WaitForNextExecutionTime(CancellationToken stoppingToken)
        {
            var now = DateTime.Now;
            var nextRun = DateTime.Today.AddHours(2);

            if (now > nextRun)
            {
                nextRun = nextRun.AddDays(1);
            }

            var delay = nextRun - now;

            _logger.LogInformation($"Prochaine anonymisation planifiée: {nextRun:yyyy-MM-dd HH:mm:ss}");

            if (delay > TimeSpan.Zero)
            {
                await Task.Delay(delay, stoppingToken);
            }
        }
    }
}