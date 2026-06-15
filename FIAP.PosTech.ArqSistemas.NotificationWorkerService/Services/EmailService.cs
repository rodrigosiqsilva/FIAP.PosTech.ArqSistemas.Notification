using FIAP.PosTech.ArqSistemas.NotificationWS;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FIAP.PosTech.ArqSistemas.NotificationWS.Services
{

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _remetenteEmail;
        private readonly string _remetenteSenha;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _smtpServer = configuration["Smtp:Server"];
            _smtpPort = int.Parse(configuration["Smtp:Port"]);
            _remetenteEmail = configuration["Smtp:Username"];
            _remetenteSenha = configuration["Smtp:Password"];
        }

        public async Task EnviarBoasVindasAsync(string destinatarioEmail, string nomeJogador)
        {
            Console.WriteLine("🎮 Iniciando o serviço de notificação da FIAP Cloud Games...");

         

            var emailService = new EmailServiceMailKit(_smtpServer, _smtpPort, _remetenteEmail, _remetenteSenha);

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

        public async Task EnviarEmailParabensPelaCompraAsync(string destinatarioEmail, string nomeJogador, string nomeJogo)
        {
            Console.WriteLine("🎮 Iniciando o serviço de notificação da FIAP Cloud Games...");

            var emailService = new EmailServiceMailKit(_smtpServer, _smtpPort, _remetenteEmail, _remetenteSenha);

            Console.WriteLine($"Disparando e-mail de parabenização pela compra para: {nomeJogador} ({destinatarioEmail})...");

            try
            {
                await emailService.EnviarEmailParabensPelaCompraAsync(destinatarioEmail, nomeJogador, nomeJogo);
                Console.WriteLine("🚀 Processo de envio de e-mail concluído com sucesso para {0}!", destinatarioEmail);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Falha no envio do e-mail de parabenização pela compra para {destinatarioEmail}: {ex.Message}");
            }
        }
    }
}