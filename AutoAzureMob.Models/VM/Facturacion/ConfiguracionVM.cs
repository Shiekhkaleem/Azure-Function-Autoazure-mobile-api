using AutoAzureMob.Models.Models.Facturacion;
using AutoAzureMob.Models.Models.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.VM.Facturacion
{
    public class ConfiguracionVM
    {
        public ConfiguracionSetting ConfiguracionSetting { get; set; } = new ConfiguracionSetting();
        public List<DropDown> ConfigFormadePagoList { get; set; } = new List<DropDown>();
    }
}
