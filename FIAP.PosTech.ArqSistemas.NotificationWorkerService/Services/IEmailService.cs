using System.Threading.Tasks;

namespace FIAP.PosTech.ArqSistemas.NotificationWS.Services
{
    public interface IEmailService
    {
        Task EnviarBoasVindasAsync(string destinatarioEmail, string nomeJogador);
        Task EnviarEmailParabensPelaCompraAsync(string destinatarioEmail, string nomeJogador, string nomeJogo);
    }
}