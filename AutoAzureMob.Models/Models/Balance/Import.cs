using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.Balance
{
    public class Import
    {
        public string MarketPlace { get; set; }
        public long ID { get; set; }
        public string Cuenta { get; set; }
        public string Tipo { get; set; }
        public string URLArchivo { get; set; }
        public string Archivo { get; set; }
        public string NombreArchivo { get; set; }
        public string Contenedor { get; set; }
        public string FechaPeticion { get; set; }
        [JsonIgnore]
        public string EstadoValue { get; set; }
        public string Estado { get; set; }
        public string EstadoValidar { get; set; }
        public string Porcentaje { get; set; }
        public string FechaActualizacion { get; set; }
        public string ArchivoError { get; set; }
        public int ChannelId { get; set; }
        public string ChannelThumbnail { get; set; }
        public int TotalRows { get; set; }
    }
}
