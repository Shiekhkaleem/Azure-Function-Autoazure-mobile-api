using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.User
{
    public class LoginRequest
    {
        public string CompanyCode { get; set;}
        public string UserName { get; set;}
        public string Password { get; set;}
    }
}
