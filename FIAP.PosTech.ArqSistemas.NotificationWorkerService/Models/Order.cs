using System;
using System.Collections.Generic;
using System.Text;

namespace FIAP.PosTech.ArqSistemas.NotificationWS.Models
{

    enum OrderStatus
    {
        Approved,
        Rejected
    }
    public class Order
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string Usuario { get; set; }
        public int IdGame { get; set; }
        public string Game { get; set; }
        public decimal Preco { get; set; }
        public string EmailUser { get; set; }
        public OrderStatus Status { get; set; }
    }
}
