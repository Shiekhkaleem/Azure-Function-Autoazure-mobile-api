using AutoAzureMob.Models.VM.OmniChannelVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.DTO.OmniDTO
{
    public class OmniRequestDTO
    {
        public string CompanyId { get; set; }
        public string ProductId { get; set; }
        public int ChannelId { get; set; }
        public bool SyncStocks { get; set; }
        public bool SyncPrices { get; set; }
        public List<Price> Prices { get; set; } = new List<Price>();
    }
}
