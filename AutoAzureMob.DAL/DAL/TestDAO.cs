using AutoAzureMob.Models.Models.Response;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.DAL.DAL
{
    public class TestDAO : BaseDAO
    {
        private readonly IConfiguration config;
        public TestDAO(ExecuteContext executionContext, IConfiguration _config) : base(executionContext, _config) 
        {
            config = _config;
        }
        public string TestFlow(string test)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
             new SqlParameter("@FirstName",test)
            };
            string queryName = "TEST_AddPerson";
            string response = ExecuteNonQuery(ExecutionContext, queryName, param, true).ToString();
            return response;
        }
    }
}
