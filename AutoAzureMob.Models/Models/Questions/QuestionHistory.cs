using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.Questions
{
    public class QuestionHistory
    {
        public string ID { get; set; }
        public string Message { get; set; }
        public string Date { get; set; }
        public int UserMKTId { get; set; }
        public bool IsAdmin { get; set; }
    }
}
