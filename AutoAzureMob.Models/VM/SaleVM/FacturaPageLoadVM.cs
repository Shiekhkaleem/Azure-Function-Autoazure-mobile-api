using AutoAzureMob.Models.Models.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.VM.SaleVM
{
    public class FacturaPageLoadVM
    {
        public List<Regimen> RegimenFiscalList { get; set; } = new List<Regimen>();
        public List<Emisor> EmisorList { get; set; } = new List<Emisor>();
        public List<DropDownV2> UsoDeCFDIList { get; set; } = new List<DropDownV2>();
        public List<DropDown> FormaDePagoList { get; set; } = new List<DropDown>();
        public List<DropDownV2> MetodoDePagoList { get; set; } = new List<DropDownV2>();

    }
}
