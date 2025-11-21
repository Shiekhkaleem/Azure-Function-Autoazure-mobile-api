using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.OmniChannel
{
    public class OmniRequest
    {
        public string CompanyId { get; set; }
        public string ListedIn { get; set; }
        public string SearchText { get; set; }
        public int Page { get; set; }
    }
}
