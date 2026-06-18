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
        private readonly IEmailService _emailService;

        // Adicionamos IConfiguration no construtor
        public PaymentProcessedEventConsumer(string bootstrapServers, string topicName, string groupId, IConfiguration configuration, IEmailService emailService)
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

            await Task.Yield();

            await Task.Run(async () =>
            {
                Console.WriteLine($"[Kafka] Iniciado loop de consumo para o tópico: {_topicName}");

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = _consumer.Consume(cancellationToken);

                        if (consumeResult != null)
                        {
                            var message = consumeResult.Message.Value;
                            Console.WriteLine($"[Kafka] Mensagem recebida no tópico {_topicName}: {message}");

                            var order = JsonSerializer.Deserialize<PaymentProcessedCreatedEvent>(message);

                            if (order != null)
                            {
                                await _emailService.EnviarEmailParabensPelaCompraAsync(
                                    order.Order.EmailUser,
                                    order.Order.Usuario,
                                    order.Order.Game
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
                        Console.WriteLine($"[Erro Geral] Erro processando evento de pagamento: {ex.Message}");
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
