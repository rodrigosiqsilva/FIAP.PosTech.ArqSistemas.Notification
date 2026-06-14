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

        // Adicionamos IConfiguration no construtor
        public UserEventConsumer(string bootstrapServers, string topicName, string groupId, IConfiguration configuration)
        {
            _topicName = topicName;
            _configuration = configuration;

            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
        }

        public Task StartConsumingAsync(CancellationToken cancellationToken)
        {
            _consumer.Subscribe(_topicName);

            // Marcamos a Action como async para podermos usar o "await" lá embaixo no EmailService
            return Task.Run(async () =>
            {
                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        try
                        {
                            var consumeResult = _consumer.Consume(cancellationToken);

                            if (consumeResult != null)
                            {
                                var options = new JsonSerializerOptions
                                {
                                    PropertyNameCaseInsensitive = true
                                };

                                var userEvent = JsonSerializer.Deserialize<UserCreatedEvent>(consumeResult.Message.Value, options);

                                if (userEvent != null && userEvent.Usuario != null)
                                {
                                    int usuarioId = userEvent.Usuario.Id;
                                    string nome = userEvent.Usuario.Nome;
                                    string emailUsuario = userEvent.Usuario.Email;

                                    Console.WriteLine($"[Kafka] Novo usuário recebido! ID: {usuarioId} | Nome: {nome} | E-mail: {emailUsuario}");

                                    // Instancia e dispara o serviço de e-mail usando os dados capturados
                                    var emailService = new EmailService(_configuration);

                                    // Presumo que o método Enviar() receba o e-mail do usuário e o nome
                                    await emailService.Enviar(emailUsuario, nome);
                                }
                            }
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"[Kafka Consumer] Erro ao consumir mensagem: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("[Kafka Consumer] Encerramento solicitado.");
                }
                finally
                {
                    _consumer.Close();
                }
            }, cancellationToken);
        }

        public void Dispose()
        {
            _consumer?.Dispose();
        }
    }
}