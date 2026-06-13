using FIAP.PosTech.ArqSistemas.NotificationWS.FIAP.PosTech.ArqSistemas.NotificationWS;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FIAP.PosTech.ArqSistemas.NotificationWS.Services
{

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Enviar(string destinatarioEmail, string nomeJogador)
        {
            Console.WriteLine("🎮 Iniciando o serviço de notificação da FIAP Cloud Games...");

            string smtpServer = _configuration["Smtp:Server"];
            int smtpPort = int.Parse(_configuration["Smtp:Port"]);
            string remetenteEmail = _configuration["Smtp:Username"];
            string remetenteSenha = _configuration["Smtp:Password"];

            var emailService = new EmailServiceMailKit(smtpServer, smtpPort, remetenteEmail, remetenteSenha);

            Console.WriteLine($"Disparando e-mail de boas-vindas para: {nomeJogador} ({destinatarioEmail})...");

            try
            {
                await emailService.EnviarEmailBoasVindasAsync(destinatarioEmail, nomeJogador);
                Console.WriteLine("🚀 Processo de envio de e-mail concluído com sucesso para {0}!", destinatarioEmail);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Falha no envio do e-mail de boas-vindas para {destinatarioEmail}: {ex.Message}");
            }
        }
    }
}