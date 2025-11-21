using AutoAzureMob.DAL.DAL;
using AutoAzureMob.Models.Models.Response;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.BLL.BLL
{
    public class TestHandler : BaseHandler
    {
        private readonly TestDAO testDAO;
        private readonly IConfiguration config;
        public TestHandler(ExecuteContext executeContext, IConfiguration _config) : base(executeContext, _config)
        {
            config = _config;
            testDAO = new TestDAO(executeContext,config);
            
        }
        public ResponseModel<string> TestFlow(string test)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            response.Content = "Hello testing functions now for authentication";
         
            response.Success = true;
            throw new Exception();
            return response;
        }
        public ResponseModel<string> CheckCode()
        {
            ResponseModel<string> response = new ResponseModel<string>();
            try
            {
                bool LiveServer = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Production");
                string connectionString = LiveServer ? config.GetConnectionString("AutoAzure-PROD") : config.GetConnectionString("AutoAzure-DEV");
                response.Description = LiveServer ? connectionString : connectionString;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Content =ex.Message;
                response.Description = "Here is problem and its is in environmental enviornment is not good";
            }
            return response;

        }
    }
}
