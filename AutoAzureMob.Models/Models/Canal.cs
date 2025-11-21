using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models
{
    public class Canal
    {
        public string ID { get; set; }
        public string Thumbnail { get; set; }
        public bool Publicado { get; set; }
        public bool SyncStock { get; set; }
        public bool SyncPrice { get; set; }
    }
}
