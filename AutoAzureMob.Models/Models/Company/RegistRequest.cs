using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.Company
{
    public class RegistRequest
    {
        public string DisplayName { get; set; }
        public string CompanyName { get; set; }
        public string MainPhone { get; set; }
        public string MainEmail { get; set; }
        public string Password { get; set; }
        public string RoleId { get; set; }
        public int ModalityProfile { get; set; }
    }
}
