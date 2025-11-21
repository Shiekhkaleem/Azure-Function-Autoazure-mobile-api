using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AutoAzureMob.Models.Models
{
    public class AzureUpload
    {
        public Stream streamImage { get; set; }
        public IFormFile File { get; set; }
        public string ImageName { get; set; }
        public string Container { get; set; }
        public string Folder { get; set; }
        public string ConnectionString { get; set; }
    }
}
