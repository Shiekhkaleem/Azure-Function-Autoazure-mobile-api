using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.DTO.MessagesDTO
{
    public class MessageReplyDTO
    {
        public string UserMKTId { get; set; }
        public string BuyerId { get; set; }
        public string OrderId { get; set; }
        public string Text { get; set; }
        public List<IFormFile> Attachments { get; set; } = new List<IFormFile>();
    }
}
