using System.Net;
using System.Security.Claims;
using AutoAzureMob.API.Helper;
using AutoAzureMob.BLL.BLL;
using AutoAzureMob.DAL.DAL;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace AutoAzureMob.API.Functions
{
    public class CompanyFunction
    {
        private readonly ILogger _logger;
        private readonly IConfiguration config;
        private readonly ExecuteContext executecontext;
        private readonly CompanyHandler companyHandler;

        public CompanyFunction(ILoggerFactory loggerFactory, ExecuteContext _executecontext = null)
        {
            _logger = loggerFactory.CreateLogger<CompanyFunction>();
            executecontext = _executecontext;
            config = ConfigurationHelper.GetConfiguration();
            companyHandler = new CompanyHandler(executecontext, config);
        }

        [Function("GetAllCompanyRoles")]
        [OpenApiOperation(operationId: "CompanyFunction", tags: new[] { "Company" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        public async Task<HttpResponseData> GetAllCompanyRoles([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,FunctionContext _context)
        {
            var result = companyHandler.GetAllCompanyRoles();
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }
    }
}
