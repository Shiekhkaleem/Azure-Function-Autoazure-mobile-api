using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.DTO.QuestionsDTO
{
    public class QuestionDTO
    {
        public string UserMKTId { get; set; }
        public int Status { get; set; }
        public int SearchBy { get; set; }
        public string SearchText { get; set; }
        public int SortBy { get; set; }
        public int Page { get; set; }
    }
}
