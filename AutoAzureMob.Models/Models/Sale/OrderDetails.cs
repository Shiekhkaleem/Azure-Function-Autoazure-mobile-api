using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.Sale
{
    public class OrderDetails
    {
        public long SaleId { get; set; }
        public string RegisterDate { get; set; }
        public int CustomerId { get; set; }
        public string Customer { get; set; }
        public string Contact { get; set; }
        public string ExpectedDate { get; set; }
        public string Store { get; set; }
        public string Email { get; set; }
        public string Seller { get; set; }
        public string Warehouse { get; set; }
        public string Reference { get; set; }
        public string Phone { get; set; }
        public string PaymentTerm { get; set; }
        public string Status { get; set; }
        public string DeliveryType { get; set; }
        public string PurchaseOrder { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Ieps { get; set; }
        public decimal Total { get; set; }
    }
}
