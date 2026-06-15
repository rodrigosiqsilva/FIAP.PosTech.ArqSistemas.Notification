using Confluent.Kafka;
using FIAP.PosTech.ArqSistemas.NotificationWS.Models;
using FIAP.PosTech.ArqSistemas.NotificationWS.Services;
using FIAP.PosTech.ArqSistemas.UserAPI.Publisher.FIAP.PosTech.ArqSistemas.UserAPI.Consumer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace FIAP.PosTech.ArqSistemas.NotificationWS.Events
{

    public record PaymentProcessedCreatedEvent(Order Order, DateTime CreatedAt, string? CorrelationId);

    public class PaymentProcessedEventConsumer : IDisposable
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly string _topicName;
        private readonly IConfiguration _configuration;

        // Adicionamos IConfiguration no construtor
        public PaymentProcessedEventConsumer(string bootstrapServers, string topicName, string groupId, IConfiguration configuration)
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

                                var paymentProcessedEvent = JsonSerializer.Deserialize<PaymentProcessedCreatedEvent>(consumeResult.Message.Value, options);

                                if (paymentProcessedEvent != null && paymentProcessedEvent.Order != null)
                                {
                                    string nome = paymentProcessedEvent.Order.Usuario;
                                    string game = paymentProcessedEvent.Order.Game;
                                    decimal preco = paymentProcessedEvent.Order.Preco;
                                    string emailUsuario = paymentProcessedEvent.Order.EmailUser;

                                    Console.WriteLine($"[Kafka] Novo pedido recebido! Usuário: {nome} | Jogo: {game} | Preço: {preco}");

                                    var emailService = new EmailService(_configuration);

                                    await emailService.EnviarEmailParabensPelaCompraAsync(emailUsuario, nome, game);
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
