using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.DTO.FacturacionDTO
{
    public class PermissionDTO
    {
        public int CompanyId { get; set; }
        public int EditUserId { get; set; }
        public int PermissionId { get; set; }
        public bool Active { get; set; }
    }
}
