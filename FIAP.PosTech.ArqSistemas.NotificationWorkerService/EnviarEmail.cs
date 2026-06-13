using FIAP.PosTech.ArqSistemas.NotificationWS.FIAP.PosTech.ArqSistemas.NotificationWS;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIAP.PosTech.ArqSistemas.NotificationWS
{

    public static class Email
    {

        public static async Task EnviarMailKit()
        {
            Console.WriteLine("🎮 Iniciando o serviço de notificação da FIAP Cloud Games...");

            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587; 

            string remetenteEmail = "fiapcloudgames5@gmail.com";
            //string remetenteSenha = "vhgx yaaj yjgl lfnw";
            string remetenteSenha = "fiap@123";

            // 3. Dados do destinatário do teste
            string destinatarioEmail = "rodrigosiqueirasilva@hotmail.com";
            string nomeJogador = "Rodrigo Siqueira";

            // Instancia o serviço de e-mail passando as configurações via construtor
            var emailService = new EmailServiceMailKit(smtpServer, smtpPort, remetenteEmail, remetenteSenha);

            Console.WriteLine($"Disparando e-mail de boas-vindas para: {nomeJogador} ({destinatarioEmail})...");

            // 4. Executa o envio assíncrono
            try
            {
                await emailService.EnviarEmailBoasVindasAsync(destinatarioEmail, nomeJogador);
                Console.WriteLine("🚀 Processo de envio concluído com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Falha crítica no fluxo da aplicação: {ex.Message}");
            }
        }
    }
}