using System;
using System.Collections.Generic;
using System.Text;

namespace FIAP.PosTech.ArqSistemas.NotificationWS
{
    using System;
    using System.Net;
    using System.Net.Mail;
    using System.Text;

    public class EmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _senderEmail;
        private readonly string _senderPassword;

        /// <summary>
        /// Construtor para parametrizar as credenciais e servidor de e-mail de envio.
        /// </summary>
        public EmailService(string smtpServer, int smtpPort, string senderEmail, string senderPassword)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _senderEmail = senderEmail;
            _senderPassword = senderPassword;
        }

        /// <summary>
        /// Envia o e-mail de boas-vindas parametrizado com o nome do integrante.
        /// </summary>
        /// <param name="emailDestinatario">E-mail do novo usuário/integrante.</param>
        /// <param name="nomeIntegrante">Nome do integrante que receberá as boas-vindas.</param>
        public void EnviarEmailBoasVindas(string emailDestinatario, string nomeIntegrante)
        {
            try
            {
                // Configuração do remetente com um nome de exibição amigável
                var de = new MailAddress(_senderEmail, "FIAP Cloud Games");
                var para = new MailAddress(emailDestinatario);

                var message = new MailMessage(de, para)
                {
                    Subject = "🎮 Prepare o seu Setup! Bem-vindo à FIAP Cloud Games",
                    Body = ObterTemplateHtmlBoasVindas(nomeIntegrante),
                    IsBodyHtml = true, // Define que o corpo do e-mail aceita HTML
                    BodyEncoding = Encoding.UTF8
                };

                using (var client = new SmtpClient(_smtpServer, _smtpPort))
                {
                    client.Credentials = new NetworkCredential(_senderEmail, _senderPassword);
                    client.EnableSsl = true; // Garante a segurança na transmissão

                    client.Send(message);
                    Console.WriteLine($"E-mail de boas-vindas enviado com sucesso para {nomeIntegrante}!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar o e-mail: {ex.Message}");
                // Em produção, trate o erro ou faça o log adequado
            }
        }

        /// <summary>
        /// Retorna o template HTML moderno com visual gamer/tecnológico.
        /// </summary>
        private string ObterTemplateHtmlBoasVindas(string nomeIntegrante)
        {
            return $@"
                <!DOCTYPE html>
                <html lang='pt-BR'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Bem-vindo à FIAP Cloud Games</title>
                    <style>
                        body {{
                            font-family: 'Segoe UI', Roboto, Helvetica, Arial, sans-serif;
                            background-color: #0a0b10;
                            color: #e2e8f0;
                            margin: 0;
                            padding: 0;
                        }}
                        .wrapper {{
                            width: 100%;
                            background-color: #0a0b10;
                            padding: 40px 0;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: 0 auto;
                            background-color: #11131e;
                            border-radius: 16px;
                            overflow: hidden;
                            box-shadow: 0 10px 30px rgba(0, 243, 255, 0.15);
                            border: 1px solid #202437;
                        }}
                        .header {{
                            background: linear-gradient(135deg, #0f172a 0%, #1e1b4b 100%);
                            padding: 45px 20px;
                            text-align: center;
                            border-bottom: 4px solid #00f3ff;
                        }}
                        .header h1 {{
                            color: #ffffff;
                            margin: 0;
                            font-size: 30px;
                            font-weight: 800;
                            text-transform: uppercase;
                            letter-spacing: 3px;
                            text-shadow: 0 0 20px rgba(0, 243, 255, 0.6);
                        }}
                        .content {{
                            padding: 40px 35px;
                            line-height: 1.7;
                        }}
                        .content h2 {{
                            color: #00f3ff;
                            margin-top: 0;
                            font-size: 24px;
                            font-weight: 700;
                        }}
                        .highlight {{
                            color: #ff007f;
                            font-weight: bold;
                        }}
                        .button-container {{
                            text-align: center;
                            margin: 35px 0;
                        }}
                        .btn {{
                            background: linear-gradient(90deg, #00f3ff 0%, #ff007f 100%);
                            color: #ffffff !important;
                            padding: 16px 40px;
                            text-decoration: none;
                            font-weight: bold;
                            border-radius: 30px;
                            text-transform: uppercase;
                            letter-spacing: 1px;
                            display: inline-block;
                            box-shadow: 0 4px 20px rgba(255, 0, 127, 0.4);
                        }}
                        .features-list {{
                            background-color: #1a1d2e;
                            border-radius: 8px;
                            padding: 20px;
                            margin: 25px 0;
                            list-style-type: none;
                        }}
                        .features-list li {{
                            margin-bottom: 12px;
                            padding-left: 25px;
                            position: relative;
                        }}
                        .features-list li::before {{
                            content: '⚡';
                            position: absolute;
                            left: 0;
                            color: #00f3ff;
                        }}
                        .footer {{
                            background-color: #07080d;
                            padding: 25px;
                            text-align: center;
                            font-size: 13px;
                            color: #64748b;
                            border-top: 1px solid #1a1d2e;
                        }}
                        .footer a {{
                            color: #00f3ff;
                            text-decoration: none;
                        }}
                    </style>
                </head>
                <body>
                    <div class='wrapper'>
                        <div class='container'>
                            <div class='header'>
                                <h1>FIAP Cloud Games</h1>
                            </div>
                            <div class='content'>
                                <h2>Olá, {nomeIntegrante}! 🕹️</h2>
                                <p>Seu cadastro foi concluído com sucesso e o próximo nível da sua experiência gamer acabou de ser desbloqueado.</p>
                                <p>A partir de agora, você faz parte da elite da <span class='highlight'>FIAP Cloud Games</span>. Esqueça downloads demorados e falta de espaço no disco: seus jogos favoritos estão rodando diretamente na nuvem com ultra performance.</p>
                        
                                <div class='button-container'>
                                    <a href='https://fiapcloudgames.example.com/login' class='btn'>Entrar no Lobby</a>
                                </div>

                                <p><strong>O que espera por você no seu primeiro login:</strong></p>
                                <ul class='features-list'>
                                    <li>Acesso instantâneo a uma biblioteca rotativa de jogos de última geração.</li>
                                    <li>Compatibilidade total com seus controles favoritos via Bluetooth ou USB.</li>
                                    <li>Salvamento automático (Cloud Save) direto na sua conta.</li>
                                </ul>
                        
                                <p>Caso precise de suporte ou queira encontrar outros membros para fechar o seu Squad, nossa comunidade e guilda de atendimento estão sempre online.</p>
                                <p>Nos vemos no jogo,<br><strong>Equipe FIAP Cloud Games</strong></p>
                            </div>
                            <div class='footer'>
                                <p>Este é um e-mail automático enviado pela plataforma FIAP Cloud Games.</p>
                                <p>&copy; 2026 FIAP Cloud Games. Todos os direitos reservados.</p>
                                <p><a href='#'>Termos de Uso</a> | <a href='#'>Central de Ajuda</a></p>
                            </div>
                        </div>
                    </div>
                </body>
                </html>
                ";
        }
    }
}
