using FIAP.PosTech.ArqSistemas.UserAPI.Publisher.FIAP.PosTech.ArqSistemas.UserAPI.Consumer;

namespace FIAP.PosTech.ArqSistemas.NotificationWS.Workers
{
    public class KafkaConsumerWorker : BackgroundService
    {
        private readonly UserEventConsumer _consumer;

        public KafkaConsumerWorker(IConfiguration configuration)
        {
            var bootstrapServers = configuration["KafkaConfig:BootstrapServers"];
            var topicName = configuration["KafkaConfig:TopicName"];
            var groupId = configuration["KafkaConfig:GroupId"];

            // Passamos a configuração para o consumer também, 
            // pois ele precisará dela para instanciar o EmailService
            _consumer = new UserEventConsumer(bootstrapServers, topicName, groupId, configuration);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("[Worker] Iniciando o consumo de mensagens do Kafka...");

            // Inicia o loop de consumo. O código vai ficar rodando "preso" aqui dentro do loop.
            await _consumer.StartConsumingAsync(stoppingToken);
        }

        public override void Dispose()
        {
            _consumer.Dispose();
            base.Dispose();
        }
    }
}