using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.Balance
{
    public class Export
    {
        public string MarketPlace { get; set; }
        public string  Reporte { get; set; }
        public string Cuenta { get; set; }
        [JsonIgnore]
        public string Filtro { get; set; }
        public string CompanyId { get; set; }
        public List<string> SaleIds { get; set; }
        public string FechaPeticion { get; set; }
        public string Estado { get; set; }
        public string Porcentaje { get; set; }
        public string FechaActualizacion { get; set; }
        [JsonIgnore]
        public string OutPut { get; set; }
        public string PdfUrl { get; set; }
        public int TotalRows { get; set; }
    }
}
