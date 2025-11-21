using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.DTO.SaleDTO
{
    public class RequestDTO
    {
        public string Channel { get; set; }
        public string ReportName { get; set; }
        public int CompanyId { get; set; }
        public int UserMKTId { get; set; }
        public string MKTAccountId { get; set; }
        public string MKTAccountNickName { get; set; }
        public string SaleId { get; set; }
        [JsonIgnore]
        public string Filters { get; set; }
    }
}
