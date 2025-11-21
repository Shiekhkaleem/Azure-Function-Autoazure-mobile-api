using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.DTO.MessagesDTO
{
    public class FilterationDTO
    {
        public long UserMKTId { get; set; }
        public int StatusId { get; set; }
        public int SubStatus { get; set; }
        public int FilterBy { get; set; }
        public string FilterText { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
    }
}
