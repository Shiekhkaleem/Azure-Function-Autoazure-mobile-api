using System.Net;
using AutoAzureMob.API.Helper;
using AutoAzureMob.BLL.BLL;
using AutoAzureMob.DAL.DAL;
using AutoAzureMob.Models.Models.Balance;
using AutoAzureMob.Models.Models.OmniChannel;
using AutoAzureMob.Models.Models.Response;
using AutoAzureMob.Models.VM.Balance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace AutoAzureMob.API.Functions
{
    public class BalanceFunction
    {
        private readonly IConfiguration config;
        private readonly ExecuteContext executecontext;
        private readonly ILogger _logger;
        private readonly BalanceHandler balanceHandler;

        public BalanceFunction(ILoggerFactory loggerFactory, ExecuteContext _executecontext = null)
        {
            _logger = loggerFactory.CreateLogger<BalanceFunction>();
            executecontext = _executecontext;
            config = ConfigurationHelper.GetConfiguration();
            balanceHandler = new BalanceHandler(executecontext,config);
        }

        [Function("GetCardPaymentOrder")]
        [OpenApiOperation(operationId: "GetCardPaymentOrder", tags: new[] { "Balance" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiParameter(name: "CompanyId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        [OpenApiParameter(name: "Page", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        public async Task<HttpResponseData> GetCardPaymentOrder([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("GetCardPaymentOrder.");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            int companyId = Convert.ToInt32(queryParam["CompanyId"]);
            int page = Convert.ToInt32(queryParam["Page"]);
            ResponseModel<PaymentOrderVM> result = balanceHandler.GetCardPaymentOrder(companyId,page);
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetExportList")]
        [OpenApiOperation(operationId: "GetExportList", tags: new[] { "Balance" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        [OpenApiParameter(name: "Page", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        public async Task<HttpResponseData> GetExportList([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("GetExportList.");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            int UserId = Convert.ToInt32(queryParam["UserId"]);
            int page = Convert.ToInt32(queryParam["Page"]);
            ResponseModel<List<Export>> result = balanceHandler.GetExportList(UserId,page);
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetImportList")]
        [OpenApiOperation(operationId: "GetImportList", tags: new[] { "Balance" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        [OpenApiParameter(name: "Page", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        public async Task<HttpResponseData> GetImportList([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("GetImportList.");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            int UserId = Convert.ToInt32(queryParam["UserId"]);
            int Page = Convert.ToInt32(queryParam["Page"]);
            ResponseModel<List<Import>> result = balanceHandler.GetImportList(UserId, Page);
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetUserAccountList")]
        [OpenApiOperation(operationId: "GetUserAccountList", tags: new[] { "Balance" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        public async Task<HttpResponseData> GetUserAccountList([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("GetUserAccountList.");
            ResponseModel<List<UserAccount>> result = balanceHandler.GetUserAccountList();
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("DownloadSaldoXml")]
        [OpenApiOperation(operationId: "DownloadSaldoXml", tags: new[] { "Balance" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiParameter(name: "InvoiceId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        public async Task<HttpResponseData>  DownloadSaldoXml([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("DownloadSaldoXml");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            int invoiceId = Convert.ToInt32(queryParam["InvoiceId"]);
            ResponseModel<string> result = balanceHandler.DownloadSaldoXml(invoiceId);
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;

        }

        [Function("DownloadSaldoPdf")]
        [OpenApiOperation(operationId: "DownloadSaldoPdf", tags: new[] { "Balance" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiParameter(name: "InvoiceId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        public async Task<HttpResponseData> DownloadSaldoPdf([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("DownloadSaldoPdf");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            int invoiceId = Convert.ToInt32(queryParam["InvoiceId"]);
            ResponseModel<string> result = balanceHandler.DownloadSaldoPdf(invoiceId);
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;

        }

        [Function("GetPagarUrl")]
        [OpenApiOperation(operationId: "GetPagarUrl", tags: new[] { "Balance" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiParameter(name: "CompanyId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "")]
        [OpenApiParameter(name: "OrderId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "")]
        [OpenApiParameter(name: "RedirectURL", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "")]
        public async Task<HttpResponseData> GetPagarUrl([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("GetPagarUrl.");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            string compnayId = queryParam["CompanyId"].ToString();
            string orderId = queryParam["OrderId"].ToString();
            string redirectUrl = queryParam["RedirectURL"].ToString()?? string.Empty;
            ResponseModel<string> result = balanceHandler.GetPagarUrl(compnayId, orderId, redirectUrl);
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }
    }
}
