using AutoAzureMob.Models.Models.Facturacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.VM.Facturacion
{
    public class UserRelacionVM
    {
        public List<Relacion> Relacion { get; set; } = new();
        public List<RelacionField> RelacionField { get; set; } = new();
    }
}
