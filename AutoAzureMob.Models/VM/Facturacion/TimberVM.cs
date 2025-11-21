using AutoAzureMob.Models.Models.Facturacion;
using AutoAzureMob.Models.Models.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.VM.Facturacion
{
    public class TimberVM
    {
        public List<TimberDropDown> TimberPerfileList { get; set; } = new List<TimberDropDown>();
        public List<DropDown> TimberList { get; set; } = new List<DropDown>();
    }
}
