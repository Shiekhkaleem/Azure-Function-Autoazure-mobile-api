using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.Sale
{
    public class InvioiceLink
    {
        public string Folio { get; set; }
        public string Reference { get; set; }
        public string ExpirationDate { get; set; }
        public int ExpDays { get; set; }
        public string LinkUrl { get; set; }
    }
}
