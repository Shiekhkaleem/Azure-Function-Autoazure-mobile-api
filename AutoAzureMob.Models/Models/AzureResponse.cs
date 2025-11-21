using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models
{
    public class AzureResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public string Success { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Content { get; set; }
    }
}
