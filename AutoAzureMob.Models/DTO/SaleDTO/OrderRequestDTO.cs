using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.DTO.SaleDTO
{
    public class OrderRequestDTO
    {
        public long CompanyId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string InvoiceTypeId { get; set; }
        public string DeliveryTypeId { get; set; }
        public string CustomerId { get; set; }
        public string References { get; set; }
        public string Channels { get; set; }
        public string Status { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
    }
}
