using AutoAzureMob.Models.Models.Balance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.VM.Balance
{
    public class PaymentOrderVM
    {
        public List<PaymentOrder> PaymentOrders { get; set; } = new List<PaymentOrder>();
        public CardBalance Card { get; set; } = new CardBalance();
    }
}
