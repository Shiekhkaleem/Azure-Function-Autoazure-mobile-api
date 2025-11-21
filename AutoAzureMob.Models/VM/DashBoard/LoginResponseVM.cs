using AutoAzureMob.Models.DTO.UserDTO;
using AutoAzureMob.Models.Models.DashBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.VM.DashBoard
{
    public class LoginResponseVM
    {
        public LoginResponseDTO LoginResponseDTO { get; set; } = new();
        public List<LinkedAccounts> LinkedAccounts { get; set; } = new();
    }
}
