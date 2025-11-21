using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.DTO.DeviceToken
{
    public  class DeviceTokenDTO
    {
        public int UserId { get; set; }
        public string DeviceToken { get; set; }
        public string Type { get; set; }
    }
}
