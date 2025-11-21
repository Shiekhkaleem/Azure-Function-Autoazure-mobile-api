using System.ComponentModel.Design;
using System.Net;
using AutoAzureMob.API.Helper;
using AutoAzureMob.BLL.BLL;
using AutoAzureMob.DAL.DAL;
using AutoAzureMob.Models.DTO.DashBoardDTO;
using AutoAzureMob.Models.DTO.UserDTO;
using AutoAzureMob.Models.Models;
using AutoAzureMob.Models.Models.DashBoard;
using AutoAzureMob.Models.Models.Response;
using AutoAzureMob.Models.VM.DashBoard;
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
    public class DashBoardFunction
    {
        private readonly ILogger _logger;
        private readonly DashBoardHandler dashBoardHandler;
        private readonly IConfiguration config;
        private readonly ExecuteContext executecontext;
        public DashBoardFunction(ILoggerFactory loggerFactory, ExecuteContext _executecontext = null)
        {
            _logger = loggerFactory.CreateLogger<DashBoardFunction>();
            executecontext = _executecontext;
            config = ConfigurationHelper.GetConfiguration();
            dashBoardHandler = new DashBoardHandler(executecontext, config);
        }

        [Function("GetDashBoardData")]
        [OpenApiOperation(operationId: "GetDashBoardData", tags: new[] { "DashBoard" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiRequestBody("application/json", typeof(DashBoardRequestDTO))]
        public async Task<HttpResponseData> GetDashBoardData([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("GetDashBoardData.");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            DashBoardRequestDTO data = JsonConvert.DeserializeObject<DashBoardRequestDTO>(requestBody);
            ResponseModel<DasBoardVM> result = dashBoardHandler.GetDashBoardData(data);
            var response = req.CreateResponse(HttpStatusCode.OK);

           await response.WriteAsJsonAsync(result);

            return response;
        }
        [Function("GetDashBoardDataV2")]
        [OpenApiOperation(operationId: "GetDashBoardDataV2", tags: new[] { "DashBoard" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiRequestBody("application/json", typeof(DashBoardRequestDTO2))]
        public async Task<HttpResponseData> GetDashBoardDataV2([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("GetDashBoardDataV2.... executing.");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            DashBoardRequestDTO2 data = JsonConvert.DeserializeObject<DashBoardRequestDTO2>(requestBody);
            ResponseModel<DashBoardVM2> result = dashBoardHandler.GetDashBoardDataV2(data);
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }
        [Function("GetLinkedAccountsList")]
        [OpenApiOperation(operationId: "GetLinkedAccountsList", tags: new[] { "DashBoard" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiRequestBody("application/json", typeof(AccountsDTO))]
        public async Task<HttpResponseData> GetLinkedAccountsList([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("GetLinkedAccountsList");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            AccountsDTO data = JsonConvert.DeserializeObject<AccountsDTO>(requestBody);
            ResponseModel<List<LinkedAccounts>> result = dashBoardHandler.GetLinkedAccountsList(data);
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }
        [Function("GetChartFiltersList")]
        [OpenApiOperation(operationId: "GetChartFiltersList", tags: new[] { "DashBoard" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        public async Task<HttpResponseData> GetChartFiltersList([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("GetChartFiltersList");
            ResponseModel<List<ChartFilters>> result = dashBoardHandler.GetChartFiltersList();
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }
        [Function("GetChartStaticsData")]
        [OpenApiOperation(operationId: "GetChartStaticsData", tags: new[] { "DashBoard" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiRequestBody("application/json", typeof(DashBoardRequestDTO))]
        public async Task<HttpResponseData> GetChartStaticsData([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("GetDashBoardData.");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            DashBoardRequestDTO data = JsonConvert.DeserializeObject<DashBoardRequestDTO>(requestBody);
            ResponseModel<List<ChartStaticsData>> result = dashBoardHandler.GetChartStaticsData(data);
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }
        [Function("GetMarketLogos")]
        [OpenApiOperation(operationId: "GetMarketLogos", tags: new[] { "DashBoard" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        public async Task<HttpResponseData> GetMarketLogos([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("GetDashBoardData.");
            ResponseModel<List<Logos>> result = dashBoardHandler.GetMarketLogos();
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }
    }
}
