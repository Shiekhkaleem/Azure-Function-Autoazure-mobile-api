using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.Balance
{
    public class PaymentOrder
    {
        public int Id { get; set; }
        public string Folio { get; set; }
        public string InvDate { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string Balance { get; set; }
        public string InvType { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string DueDate { get; set; }
        public int InvoiceId { get; set; }
        public bool NotTransfer { get; set; }
        public int OL { get; set; }
        public int TotalRows { get; set; }

    }
}
