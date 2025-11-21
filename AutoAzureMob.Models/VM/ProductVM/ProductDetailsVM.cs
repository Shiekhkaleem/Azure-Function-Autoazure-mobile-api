using AutoAzureMob.Models.Models.OmniChannel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.VM.ProductVM
{
    public class ProductDetailsVM
    {
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string Thumbnail { get; set; }
        public List<ProductDetails> ProductDetail { get; set; } = new List<ProductDetails>();
    }
}
