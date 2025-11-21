using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.Facturacion
{
    public class CompanyUser
    {
        public int UserId {  get; set; }
        public string Numero { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Store { get; set; }
        public int TotalRows { get; set; }
    }
}
