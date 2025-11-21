using AutoAzureMob.Models.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.VM.Facturacion
{
    public class PermissionTabVM
    {
        public string SubModule { get; set; }
        public List<PermissionTab> Tabs { get; set; } = new();
    }
}
