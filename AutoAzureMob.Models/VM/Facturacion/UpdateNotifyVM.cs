using AutoAzureMob.Models.DTO.FacturacionDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.VM.Facturacion
{
    public class UpdateNotifyVM
    {
        public string CompanyId { get; set; }
        public string ChannelId { get; set; }
        public string AccountId { get; set; }
        public List<UserPermissionDTO> UserModules { get; set; } = new();
        [JsonIgnore]
        public List<UsersVM> Users { get; set; } = new();
    }
    public class UsersVM
    {
        public string AccountID { get; set; }
        public List<UserPermissionDTO> Permissions { get; set; } 
    }
}
