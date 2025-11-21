using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.OmniChannel
{
    public class ProductDetails
    {
        public string ItemId { get; set; }
        public string VariationId { get; set; }
        public string Channel { get; set; }
        public int ChannelId { get; set; }
        public string Thumbnail { get; set; }
        public string UserName { get; set; }
        public string Sku { get; set; }
        public string Title { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string PubUrl { get; set; }
    }
}
