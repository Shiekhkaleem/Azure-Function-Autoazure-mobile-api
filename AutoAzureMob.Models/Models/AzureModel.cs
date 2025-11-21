using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models
{
    public class AzureModel
    {
        public AzureModel()
        {
            RequestType = "POST";
        }
        public string JSON { get; set; }
        public string Queue { get; set; }
        public string URL { get; set; }
        public string RequestType { get; set; }
        public Dictionary<string, string> WebHeaders { get; set; }
        public string Content { get; set; }
    }
}
