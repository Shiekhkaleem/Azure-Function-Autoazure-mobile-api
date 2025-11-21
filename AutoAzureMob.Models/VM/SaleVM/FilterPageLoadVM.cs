using AutoAzureMob.Models.Models.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.VM.SaleVM
{
    public class FilterPageLoadVM
    {
        public List<Channels> Channels { get; set; } = new List<Channels>();
        public List<SaleStatus> SaleStatus { get; set; } = new List<SaleStatus>();
    }
}
