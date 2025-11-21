using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.Company
{
    public class RegistResponse
    {
        public string CompanyCode { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public string CompanyBrand { get; set; }
        public string Result { get; set; }
        public string ResultText { get; set; }
        public string UniGuide { get; set; }
    }
}
