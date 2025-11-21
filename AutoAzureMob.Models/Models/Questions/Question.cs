using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.Questions
{
    public class Question
    {
        public string QuestionID { get; set; }
        public string FromID { get; set; }
        public string FromName { get; set; }
        public string Text { get; set; }
        public string ItemID { get; set; }
        public string DateCreated { get; set; }
        public string TotalDays { get; set; }
        public string Thumbnail { get; set; }
        public string AnswerText { get; set; }
        public string AnswerDateCreated { get; set; }
        public string Title { get; set; }
        public string AvailableQuantity { get; set; }
        public string Price { get; set; }
        public int TotalRows { get; set; }
        public string Permalink { get; set; }
        public string Status { get; set; }
        public string OfficialStoreName { get; set; }
        public string LogisticType { get; set; }
        public string SKU { get; set; }
        public string ShippingMode { get; set; }
    }
}
