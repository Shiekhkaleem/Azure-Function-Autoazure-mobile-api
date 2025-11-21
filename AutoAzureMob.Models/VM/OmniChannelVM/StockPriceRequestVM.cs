using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.VM.OmniChannelVM
{
    public class StockPriceRequestVM
    {
        public string CompanyId { get; set; }
        public string ProductId { get; set; }
        public List<StockPrice> SyncStocks { get; set; } = new List<StockPrice>();
        public List<StockPrice> SyncPrices { get; set; } = new List<StockPrice>();
        public List<Price> Prices { get; set; } = new List<Price>();
    }
    public class StockPrice
    {
        public string Name { get; set; }
        public string ChannelID { get; set; }
        public bool value { get; set; }
    }
    public class Price
    {
        public string Name { get; set; }
        public string ChannelID { get; set; }
        public string value { get; set; }
    }
}
