using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.Facturacion
{
    public class Profile
    {
        public string ProfileId { get; set; }
        public string TaxId { get; set; }
        public string TaxName { get; set; }
        public string Address { get; set; }
        public int TotalRows { get; set; }
    }
}
