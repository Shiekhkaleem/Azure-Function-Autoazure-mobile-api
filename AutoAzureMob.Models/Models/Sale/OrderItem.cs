using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.Sale
{
    public class OrderItem
    {
        public string SKU { get; set; }
        public string Total { get; set; }
        public string Title { get; set; }
        public string Warehouse { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal Iva { get; set; }
        public decimal Ieps { get; set; }
    }
}
