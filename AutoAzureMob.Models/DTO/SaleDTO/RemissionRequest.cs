using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.DTO.SaleDTO
{
    public class RemissionRequest
    {
        public string Type { get; set; }
        public string UserId { get; set; }
        public string CompanyId { get; set; }
        public string SaleId { get; set; }
        public string PayForm { get; set; }
        public string PayMethod { get; set; }
        public string CustomerId { get; set; }
        public string UseOfCFDI { get; set; }
        public string PayConditions { get; set; }
        public string Email { get; set; }
        public string Bank { get; set; }
        public string PayReference { get; set; }
        public string TaxName { get; set; }
        public string TaxId { get; set; }
        public string Street { get; set; }
        public string NoExt { get; set; }
        public string NoInt { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Date { get; set; }
        public string ZipCode { get; set; }
        public string RegimenFiscal { get; set; }
        public string ProfileNo { get; set; }
    }
}
