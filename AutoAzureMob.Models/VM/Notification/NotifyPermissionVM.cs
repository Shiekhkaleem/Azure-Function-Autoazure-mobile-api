using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.VM.Notification
{
    public class NotifyPermissionVM
    {
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public string ChannelId { get; set; }
        public string CompanyId { get; set; }
        public List<NotifyPermission> Permissions { get; set; } = new();
    }
    public class NotifyPermission
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ActiveWP { get; set; }
    }
}
