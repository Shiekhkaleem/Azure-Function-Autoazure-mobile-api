using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.Facturacion
{
    public class ConfiguracionSetting
    {
        public int CompanyId { get; set; }
        public bool IncludeShippingCost { get; set; }
        public bool IncludeReference { get; set; }
        public bool GenerateCreditNote { get; set; }
        public bool GlobalSkuInvoice { get; set; }
        public int ValidDays { get; set; }
        public int AllowFP { get; set; }
        public string FormaPago { get; set; }
    }
}
