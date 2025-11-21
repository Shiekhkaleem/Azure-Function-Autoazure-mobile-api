using AutoAzureMob.Models.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.API.ExceptionHandling
{
    public class ErrorHandlerMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
              HttpResponseData response = await FunctionContextExtensions.GetHttpResponseData(context);
            try
            {
                await next(context);
            }
            catch (Exception error)
            {

               switch (error) 
                {
                    case KeyNotFoundException e:
                        response.StatusCode = HttpStatusCode.NotFound;
                            break;
                    default:
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        break;
                }
                var result = new ResponseModel<ResultDTO>()
                {

                    Content = null,
                    Success = false,
                    ExceptionMessage = error.Message,
                    Description = "Algo salió mal.",
                    Title = "Error!"
                };
                await response.WriteAsJsonAsync(result);
            }
        }
    }
}
