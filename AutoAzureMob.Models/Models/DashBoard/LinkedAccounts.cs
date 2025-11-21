using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.DashBoard
{
    public class LinkedAccounts
    {
        public int CompanyId { get; set; }
        public string UserMKTId { get; set; }
        public string StoreName { get; set; }
        public int ChannelId { get; set; }
        public string ChannelThumbnail { get; set; }
        public string ChannelName { get; set; }
        public bool DashBoard { get; set; }
    }
}
