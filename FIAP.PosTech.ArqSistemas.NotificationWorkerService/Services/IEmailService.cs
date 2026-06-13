using System;
using System.Collections.Generic;
using System.Text;

namespace FIAP.PosTech.ArqSistemas.NotificationWS.Services
{
    public interface IEmailService
    {
        Task Enviar(string destinatarioEmail, string nomeJogador);
    }
}
