using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.DTO.NotiDTO
{
    public class NotificationDTO
    {
        public string ID { get; set; }
        public string Type { get; set; }
        public string CatalogID { get; set; }
        public string DateCreated { get; set; }
        public string Channel { get; set; }
        public string Logo { get; set; }
        public string Color { get; set; }
        public string AccountID { get; set; }
        public string NickName { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Message { get; set; }
        public string Picture { get; set; }
        public string Status { get; set; }
        public string ResourceID { get; set; }
        public string ResourceName { get; set; }
        public string CompanyID { get; set; }
        public string Total { get; set; }
    }
}
