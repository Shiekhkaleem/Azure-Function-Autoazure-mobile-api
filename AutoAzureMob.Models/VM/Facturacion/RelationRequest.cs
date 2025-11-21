using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.VM.Facturacion
{
    public class RelationRequest
    {
        public string CompanyId { get; set; }
        public string ChannelId { get; set; }
        public List<UserProfile> IstUserProfile { get; set; }
    }
    public class UserProfile
    {
        public string mktuserid { get; set; }
        public string profileid { get; set; }
    }
}
