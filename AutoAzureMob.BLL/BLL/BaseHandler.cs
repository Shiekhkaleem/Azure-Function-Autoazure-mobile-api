using AutoAzureMob.DAL.DAL;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoAzureMob.BLL.BLL
{
    public class BaseHandler
    {
        public ExecuteContext _executeContext { get; protected set; }
        public BaseHandler(ExecuteContext executeContext,IConfiguration configuration)
        {
            if (executeContext != null && executeContext.Connection != null)
            { 
             _executeContext = executeContext;
            }
            else
            {
                _executeContext = BaseDAO.CreateExecutionContext(false);
            }
        }
    }
}
