using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.User
{
    public class UserInfo
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public string UniqueGuid { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyBrand { get; set; }
        public string ContactName { get; set; }
        public bool FirstStart { get; set; }
        public bool Expired { get; set; }
        public string LicenseStatus { get; set; }
        public int DaysLeft { get; set; }
        public string StatusName { get; set; }
        public bool IsAdmin { get; set; }
        public bool ForcePass { get; set; }
        public string Email { get; set; }
        public bool Modality { get; set; }
        public bool Comp_Mod { get; set; }
        public bool Ml_UnlinkedAccount { get; set; }
        public int DemoDaysLeft { get; set; }
        public bool InfoUpdateCFDI40 { get; set; }
    }
}
