using FIAP.PosTech.ArqSistemas.NotificationWS.Events;
using FIAP.PosTech.ArqSistemas.UserAPI.Publisher.FIAP.PosTech.ArqSistemas.UserAPI.Consumer;

namespace FIAP.PosTech.ArqSistemas.NotificationWS.Workers
{
    public class KafkaConsumerWorker : BackgroundService
    {
        private readonly UserEventConsumer _consumerUserCreated;
        private readonly PaymentProcessedEventConsumer _consumerPaymentProcessed;

        public KafkaConsumerWorker(IConfiguration configuration)
        {
            var bootstrapServers = configuration["KafkaConfig:BootstrapServers"];
            var topicNameUserCreated = configuration["KafkaConfig:TopicNameUserCreated"];
            var topicNamePaymentProcessed = configuration["KafkaConfig:TopicNamePaymentProcessed"];
            var groupId = configuration["KafkaConfig:GroupId"];

            _consumerUserCreated = new UserEventConsumer(bootstrapServers, topicNameUserCreated, groupId, configuration);
           // _consumerPaymentProcessed = new PaymentProcessedEventConsumer(bootstrapServers, topicNamePaymentProcessed, groupId, configuration);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("[Worker] Iniciando o consumo de mensagens do Kafka...");

            await _consumerUserCreated.StartConsumingAsync(stoppingToken);
            await _consumerPaymentProcessed.StartConsumingAsync(stoppingToken);
        }

        public override void Dispose()
        {
            _consumerUserCreated.Dispose();
            _consumerPaymentProcessed.Dispose();
            base.Dispose();
        }
    }
}