using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.Messages
{
    public class Message
    {
        public long OrderId { get; set; }
        public string Title { get; set; }
        public string Received { get; set; }
        public string TotalDays { get; set; }
        public string LastMessage { get; set; }
        public string IDLastMessage { get; set; }
        public string Sku { get; set; }
        public long ReadId { get; set; }
        public int OrderStatus { get; set; }
        public string OrderSubStatus { get; set; }
        public int RowNumber { get; set; }
        public int TotalRows { get; set; }

    }
}
