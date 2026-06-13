using FIAP.PosTech.ArqSistemas.NotificationWS;
using FIAP.PosTech.ArqSistemas.NotificationWS.FIAP.PosTech.ArqSistemas.NotificationWS;
using FIAP.PosTech.ArqSistemas.NotificationWS.Services;

namespace FIAP.PosTech.ArqSistemas.NotificationWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration; 

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                string destinatarioEmail = "rodrigosiqueirasilva@hotmail.com";
                string nomeJogador = "Rodrigo Siqueira";
                var email = new EmailService(_configuration);
                await email.Enviar(destinatarioEmail, nomeJogador);
                await Task.Delay(100000000, stoppingToken);
            }
        }
    }
}
