using Confluent.Kafka;
using FIAP.PosTech.ArqSistemas.NotificationWS.Models;
using FIAP.PosTech.ArqSistemas.NotificationWS.Services; // Para o EmailService
using System.Text.Json;

namespace FIAP.PosTech.ArqSistemas.UserAPI.Publisher.FIAP.PosTech.ArqSistemas.UserAPI.Consumer
{
    public record UserCreatedEvent(User Usuario, DateTime CreatedAt, string? CorrelationId);

    public class UserEventConsumer : IDisposable
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly string _topicName;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        // Adicionamos IConfiguration no construtor
        public UserEventConsumer(string bootstrapServers, string topicName, string groupId, IConfiguration configuration, IEmailService emailService)
        {
            _topicName = topicName;
            _configuration = configuration;
            _emailService = emailService;

            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
        }

        public async Task StartConsumingAsync(CancellationToken cancellationToken)
        {
            _consumer.Subscribe(_topicName);

            // Garante que o método yields (retorna o controle) imediatamente para o Worker
            await Task.Yield();

            // Executa em uma thread do ThreadPool dedicada
            await Task.Run(async () =>
            {
                Console.WriteLine($"[Kafka] Iniciado loop de consumo para o tópico: {_topicName}");

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        // Consume é bloqueante, mas rodando dentro do Task.Run isolado, tudo bem.
                        // Passamos o timeout pequeno ou o próprio token para não travar infinitamente.
                        var consumeResult = _consumer.Consume(cancellationToken);

                        if (consumeResult != null)
                        {
                            var message = consumeResult.Message.Value;
                            Console.WriteLine($"[Kafka] Mensagem recebida no tópico {_topicName}: {message}");

                            var user = JsonSerializer.Deserialize<UserCreatedEvent>(message);

                            if (user != null)
                            {
                                await _emailService.EnviarBoasVindasAsync(
                                    user.Usuario.Email,
                                    user.Usuario.Nome
                                );
                            }
                        }
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"[Kafka Erro] Erro ao consumir mensagem: {e.Error.Reason}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Erro Geral] Erro processando evento: {ex.Message}");
                    }
                }
            }, cancellationToken);
        }

        public void Dispose()
        {
            _consumer?.Dispose();
        }
    }
}