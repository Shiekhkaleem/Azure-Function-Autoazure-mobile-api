using AutoAzureMob.API.ExceptionHandling;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.API.Logger
{
    public class LoggingMiddleware :  IFunctionsWorkerMiddleware
    {
        internal readonly log4net.ILog _log = null;
        public LoggingMiddleware()
        {
            _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);  
        }
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        
        {
            if (context is not null)
            {
                GetFunctionDetails(context);
            }
            await next(context!);
        }
        public void GetFunctionDetails(FunctionContext context)
        {
            HttpRequestData request = FunctionContextExtensions.GetHttpRequestData(context);
           string body = new StreamReader(request.Body).ReadToEnd().ToString();
        }
    }
}
