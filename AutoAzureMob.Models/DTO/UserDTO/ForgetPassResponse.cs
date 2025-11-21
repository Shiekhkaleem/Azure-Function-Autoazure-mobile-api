using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.DTO.UserDTO
{
    public class ForgetPassResponse
    {
        public string Result { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string MainContact { get; set; }
    }
}
