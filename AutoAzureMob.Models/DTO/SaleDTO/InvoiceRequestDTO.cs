using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.DTO.SaleDTO
{
    public class InvoiceRequestDTO
    {
        public int SaleId { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
