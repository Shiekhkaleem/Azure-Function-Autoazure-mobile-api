using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.OmniChannel
{
    public class ProductInfo
    {
        public string Sku { get; set; }
        public string Brand { get; set; }
        public string Title { get; set; }
        public decimal Stock { get; set; }
        public string SyncStocks { get; set; }
        public string SyncPrices { get; set; }
        public string Prices { get; set; }
    }
}
