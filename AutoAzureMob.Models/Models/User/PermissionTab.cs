using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.User
{
    public class PermissionTab
    {
        public int PermissionId { get; set; }
        public string SubModule { get; set; }
        public string PermissionName { get; set; }
        public string PermissionDescription { get; set; }
        public bool Active { get; set; }
    }
}
