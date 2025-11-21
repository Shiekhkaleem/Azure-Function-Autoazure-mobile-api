using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.OmniChannel
{
    public class OmniChannel
    {
        public int Product { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public int Stock { get; set; }
        public int TotalCanals { get; set; }
        public int TotalPrice { get; set; }
        public int TotalStock { get; set; }
        public List<Canal> Canales { get; set; } = new List<Canal>();
        public int TotalCount { get; set; }
        [JsonIgnore]
        public string Sincronizacion { get; set; }
    }
}
