using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.User
{
    public class UserRegistration
    {
        public string UserId { get; set; }
        public string CompanyCode { get; set; }
        public string FirstLastName { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Brand { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Profile { get; set; }
        public int Source { get; set; }
    }
}
