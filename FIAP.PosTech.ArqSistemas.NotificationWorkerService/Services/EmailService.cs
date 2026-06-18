using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace FIAP.PosTech.ArqSistemas.NotificationWS.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailServiceMailKit _mailKitService;

        // O construtor agora recebe a instância pronta via Injeção de Dependência
        public EmailService(IConfiguration configuration)
        {
            var smtpServer = configuration["Smtp:Server"]
                ?? throw new ArgumentNullException("Smtp:Server não configurado.");
            var smtpPort = int.Parse(configuration["Smtp:Port"] ?? "587");
            var remetenteEmail = configuration["Smtp:Username"]
                ?? throw new ArgumentNullException("Smtp:Username não configurado.");
            var remetenteSenha = configuration["Smtp:Password"]
                ?? throw new ArgumentNullException("Smtp:Password não configurado.");

            // Inicializa o motor do MailKit uma única vez
            _mailKitService = new EmailServiceMailKit(smtpServer, smtpPort, remetenteEmail, remetenteSenha);
        }

        public async Task EnviarBoasVindasAsync(string destinatarioEmail, string nomeJogador)
        {
            Console.WriteLine($"[EmailService] Iniciando disparo de boas-vindas para: {nomeJogador} ({destinatarioEmail})...");

            try
            {
                await _mailKitService.EnviarEmailBoasVindasAsync(destinatarioEmail, nomeJogador);
                Console.WriteLine($"🚀 E-mail de boas-vindas enviado com sucesso para {destinatarioEmail}!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Falha ao enviar boas-vindas para {destinatarioEmail}: {ex.Message}");
                throw; // Propaga o erro para o consumidor registrar no log do Kafka se necessário
            }
        }

        public async Task EnviarEmailParabensPelaCompraAsync(string destinatarioEmail, string nomeJogador, string nomeJogo)
        {
            Console.WriteLine($"[EmailService] Iniciando disparo de compra para: {nomeJogador} ({destinatarioEmail})...");

            try
            {
                await _mailKitService.EnviarEmailParabensPelaCompraAsync(destinatarioEmail, nomeJogador, nomeJogo);
                Console.WriteLine($"🚀 E-mail de parabéns pela compra enviado com sucesso para {destinatarioEmail}!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Falha ao enviar e-mail de compra para {destinatarioEmail}: {ex.Message}");
                throw;
            }
        }
    }
}