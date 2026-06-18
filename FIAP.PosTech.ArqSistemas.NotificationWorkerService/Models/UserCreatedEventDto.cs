using System;
using System.Collections.Generic;
using System.Text;

namespace FIAP.PosTech.ArqSistemas.NotificationWS.Models
{
    public class UserCreatedEventDto
    {
        public UserDto User { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CorrelationId { get; set; }      
    }
}
