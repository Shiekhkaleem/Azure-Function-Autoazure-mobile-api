using AutoAzureMob.Models.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.DTO.UserDTO
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public string Key { get; set; }
        public bool IsFresh { get; set; }
        public UserInfo UserInfo { get; set; } = new UserInfo();
    }
}
