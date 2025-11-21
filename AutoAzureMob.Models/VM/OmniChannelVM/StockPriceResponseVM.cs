using AutoAzureMob.Models.Models.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.VM.OmniChannelVM
{
    public class StockPriceResponseVM
    {
        public string Sku { get; set; }
        public string Brand { get; set; }
        public string Title { get; set; }
        public decimal Stock { get; set; }
        public List<StockPriceVM> StockPriceVM { get; set; } = new List<StockPriceVM>();
        [JsonIgnore]
        public List<StockPrice> SyncStocks { get; set; } = new List<StockPrice>();
        [JsonIgnore]
        public List<StockPrice> SyncPrices { get; set; } = new List<StockPrice>();
        [JsonIgnore]
        public List<Price> Prices { get; set; } = new List<Price>();
        public List<Channels> Channels { get; set; } = new List<Channels>();
    }
    public class StockPriceVM
    {
        public int ChannelId { get; set; }
        public int ChannelName { get; set; }
        public bool SyncStocks { get; set; }
        public bool SyncPrices { get; set; }
        public List<Price> Prices { get; set; } = new List<Price>();
    }
}
