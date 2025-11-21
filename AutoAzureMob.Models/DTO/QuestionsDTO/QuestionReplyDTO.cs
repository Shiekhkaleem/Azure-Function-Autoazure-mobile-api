using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.DTO.QuestionsDTO
{
    public class QuestionReplyDTO
    {
        public long QuestionId { get; set; }
        public long AnsweredBy { get; set; }
        public string Text { get; set; }
    }
}
