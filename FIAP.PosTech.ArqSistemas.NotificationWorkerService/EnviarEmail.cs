using System;
using System.Collections.Generic;
using System.Text;

namespace FIAP.PosTech.ArqSistemas.NotificationWS
{
    static void EnviarEmail()
    {
        // 1. Parametrizando os dados do servidor de e-mail de envio da plataforma
        string servidorSmtp = "smtp.gmail.com";
        int portaSmtp = 587;
        string emailRemetente = "fiapcloudgames5@gmail.com";
        string senhaRemetente = "fiap@123";

        // Inicializa o serviço com os parâmetros globais de envio
        EmailService emailService = new EmailService(servidorSmtp, portaSmtp, emailRemetente, senhaRemetente);

        // 2. Simulando dados dinâmicos dos integrantes que mudam a cada disparo
        string emailDoNovoIntegrante = "rodrigosiqueirasilva@hotmail.com";
        string nomeDoNovoIntegrante = "Rodrigo Siqueira";

        // Realiza o envio customizado
        emailService.EnviarEmailBoasVindas(emailDoNovoIntegrante, nomeDoNovoIntegrante);
    }
}