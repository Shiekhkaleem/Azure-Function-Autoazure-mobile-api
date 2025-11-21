using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.Response
{
    public class ResponseModel<T> where T : class
    {
        public ResponseModel()
        {
            Content = null;
            Success = false;
        }
        public ResponseModel(T content)
        {
            Success = true;
            Title = string.Empty;
            Description = string.Empty;
            Content = content;
            ExceptionMessage = string.Empty;    
        }
        public bool Success { get; set; }
        public string Title { get; set; }
        public string ExceptionMessage { get; set; }
        public string Description { get; set; }
        public T Content { get; set; }
    }
}
