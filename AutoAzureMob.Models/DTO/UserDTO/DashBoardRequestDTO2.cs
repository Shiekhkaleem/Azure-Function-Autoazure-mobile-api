using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.DTO.UserDTO
{
    public class DashBoardRequestDTO2
    {
        public int ChannelId { get; set; }
        public string CompanyId { get; set; }
        public string UserMKTID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int ChartType { get; set; }
    }
}
