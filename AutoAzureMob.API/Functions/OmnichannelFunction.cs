using System.Net;
using AutoAzureMob.API.Helper;
using AutoAzureMob.BLL.BLL;
using AutoAzureMob.DAL.DAL;
using AutoAzureMob.Models.DTO.OmniDTO;
using AutoAzureMob.Models.Models.OmniChannel;
using AutoAzureMob.Models.Models.Response;
using AutoAzureMob.Models.VM.OmniChannelVM;
using AutoAzureMob.Models.VM.ProductVM;
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
    public class OmnichannelFunction
    {
        private readonly ILogger _logger;
        private readonly OmnichannelHandler omnichannelHandler;
        private readonly IConfiguration config;
        private readonly ExecuteContext executecontext;

        public OmnichannelFunction(ILoggerFactory loggerFactory, ExecuteContext _executecontext = null)
        {
            _logger = loggerFactory.CreateLogger<DashBoardFunction>();
            executecontext = _executecontext;
            config = ConfigurationHelper.GetConfiguration();
            omnichannelHandler = new OmnichannelHandler(executecontext, config);
        }

        [Function("GetOmniChannelList")]
        [OpenApiOperation(operationId: "GetOmniChannelList", tags: new[] { "OmniChannel" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiRequestBody("application/json", typeof(OmniRequest))]
        public async Task<HttpResponseData> GetOmniChannelList([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("GetOmniChannelList.");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            OmniRequest data = JsonConvert.DeserializeObject<OmniRequest>(requestBody);
            ResponseModel<List<OmniChannel>> result = omnichannelHandler.GetOmniChannelList(data);
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("RelatedPublicacionsById")]
        [OpenApiOperation(operationId: "RelatedPublicacionsById", tags: new[] { "OmniChannel" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiParameter(name: "ProductId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        public async Task<HttpResponseData> RelatedPublicacionsById([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("GetOmniChannelList.");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            int productId = Convert.ToInt32(queryParam["ProductId"]);
            ResponseModel<List<ProductDetailsVM>> result = omnichannelHandler.RelatedPublicacionsById(productId);
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("UpdateSyncStockPrice")]
        [OpenApiOperation(operationId: "UpdateSyncStockPrice", tags: new[] { "OmniChannel" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiRequestBody("application/json", typeof(OmniRequestDTO))]
        public async Task<HttpResponseData> UpdateSyncStockPrice([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("UpdateSyncStockPrice.");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            OmniRequestDTO data = JsonConvert.DeserializeObject<OmniRequestDTO>(requestBody);
            ResponseModel<string> result = omnichannelHandler.UpdateSyncStockPrice(data);
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetProductInfo")]
        [OpenApiOperation(operationId: "GetProductInfo", tags: new[] { "OmniChannel" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiParameter(name: "CompanyId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        [OpenApiParameter(name: "ProductId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        public async Task<HttpResponseData> GetProductInfo([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("GetProductInfo.");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            int companyId = Convert.ToInt32(queryParam["CompanyId"]);
            int productId = Convert.ToInt32(queryParam["ProductId"]);
            ResponseModel<StockPriceResponseVM> result = omnichannelHandler.GetProductInfo(companyId,productId);
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }
    }
}
