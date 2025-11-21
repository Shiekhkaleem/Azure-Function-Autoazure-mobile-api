using AutoAzureMob.Models.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.VM.MessageVM
{
    public class MessageVM
    {
        public string Billing_Url { get; set; }
        public string BuyerId { get; set; }
        public List<MessageInfo> Messages { get; set; } = new List<MessageInfo>();
    }
}
