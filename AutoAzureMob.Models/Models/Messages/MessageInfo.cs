using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.Messages
{
    public class MessageInfo
    {
        public string OrderID { get; set; }
        public string UserID { get; set; }
        public string DateReceived { get; set; }
        public string Text { get; set; }  
        public string FromUserID { get; set; }
        public string ToUserID { get; set; }
        public string BuyerID { get; set; }
        public string AttachedFiles { get; set; }
        public bool IsAdmin { get; set; }
    }
}
