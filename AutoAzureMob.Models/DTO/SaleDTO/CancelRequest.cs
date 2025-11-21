using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.DTO.SaleDTO
{
    public class CancelRequest
    {
        public int SaleId { get; set; }
        public int CompanyId { get; set; }
    }
}
