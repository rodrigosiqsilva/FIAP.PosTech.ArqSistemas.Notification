using FIAP.PosTech.ArqSistemas.NotificationWS.Events;
using FIAP.PosTech.ArqSistemas.NotificationWS.Services;
using FIAP.PosTech.ArqSistemas.UserAPI.Publisher.FIAP.PosTech.ArqSistemas.UserAPI.Consumer;

namespace FIAP.PosTech.ArqSistemas.NotificationWS.Workers
{
    public class KafkaConsumerWorker : BackgroundService
    {
        private readonly UserEventConsumer _consumerUserCreated;
        private readonly PaymentProcessedEventConsumer _consumerPaymentProcessed;

        public KafkaConsumerWorker(IConfiguration configuration, IEmailService emailService)
        {
            var bootstrapServers = configuration["KafkaConfig:BootstrapServers"];
            var topicNameUserCreated = configuration["KafkaConfig:TopicNameUserCreated"];
            var topicNamePaymentProcessed = configuration["KafkaConfig:TopicNamePaymentProcessed"];
            var groupIdUserCreated = configuration["KafkaConfig:GroupIdUserCreated"];
            var groupIdPaymentProcessed = configuration["KafkaConfig:GroupIdPaymentProcessed"];

            _consumerUserCreated = new UserEventConsumer(bootstrapServers, topicNameUserCreated, groupIdUserCreated, configuration, emailService);
            _consumerPaymentProcessed = new PaymentProcessedEventConsumer(bootstrapServers, topicNamePaymentProcessed, groupIdPaymentProcessed, configuration, emailService);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("[Worker] Iniciando o consumo de mensagens do Kafka...");

            await Task.WhenAll(
                _consumerUserCreated.StartConsumingAsync(stoppingToken),
                _consumerPaymentProcessed.StartConsumingAsync(stoppingToken)
            );
        }

        public override void Dispose()
        {
            _consumerUserCreated.Dispose();
            _consumerPaymentProcessed.Dispose();
            base.Dispose();
        }
    }
}