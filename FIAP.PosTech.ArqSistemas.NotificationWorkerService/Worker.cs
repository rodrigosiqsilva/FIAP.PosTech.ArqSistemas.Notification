using FIAP.PosTech.ArqSistemas.NotificationWS;
using FIAP.PosTech.ArqSistemas.NotificationWS.FIAP.PosTech.ArqSistemas.NotificationWS;

namespace FIAP.PosTech.ArqSistemas.NotificationWorkerService
{
    public class Worker(ILogger<Worker> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await Email.EnviarMailKit();
                await Task.Delay(100000000, stoppingToken);
            }
        }
    }
}
