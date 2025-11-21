using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.Sale
{
    public class SaleOrders
    {
        public long SaleId { get; set; }
        public string Folio { get; set; }
        public string SaleDate { get; set; }
        public int CustomerId { get; set; }
        public string Customer { get; set; }
        public string Email { get; set; }
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string ChannelThumbnail { get; set; }
        public string DeliveryMethod { get; set; }
        public string Store { get; set; }
        public string WareHouse { get; set; }
        public string Reference { get; set; }
        public string Total { get; set; }
        public int InvoiceId { get; set; }
        public string Invoice { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string StatusDate { get; set; }
        public int TotalRows { get; set; }
    }
}
