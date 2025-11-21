using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.Facturacion
{
    public class UserData
    {
        public int UserId { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int StoreId { get; set; }
        public int PriceId { get; set; }
    }
}
