using System.Net;
using AutoAzureMob.API.Helper;
using AutoAzureMob.BLL.BLL;
using AutoAzureMob.DAL.DAL;
using AutoAzureMob.Models.DTO.NotiDTO;
using AutoAzureMob.Models.Models.Response;
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
    public class NotificationFunction
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly ExecuteContext _executeContext;
        private readonly NotificationHandler _notificationHandler;
        public NotificationFunction(ILoggerFactory loggerFactory, ExecuteContext executeContext)
        {
            _logger = loggerFactory.CreateLogger<NotificationFunction>();
            _config = ConfigurationHelper.GetConfiguration();
            _notificationHandler = new NotificationHandler(executeContext, _config);
        }

        [Function("DeleteDeviceToken")]
        [OpenApiOperation(operationId: "Notifications", tags: new[] { "DeleteDeviceToken" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiParameter(name: "type", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "")]
        [OpenApiParameter(name: "companyId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "")]
        public async Task<HttpResponseData> DeleteDeviceToken([HttpTrigger(AuthorizationLevel.Function, "delete")] HttpRequestData req, FunctionContext context)
        {
            _logger.LogInformation("DeleteDeviceToken");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            string type = queryParam["type"].ToString();
            string companyId = queryParam["companyId"].ToString();
            ResponseModel<string> result = _notificationHandler.RemoveUserDeviceToken(Convert.ToInt32(companyId), type);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("MarkSingleNotifyAsRead")]
        [OpenApiOperation(operationId: "MarkSingleNotifyAsRead", tags: new[] { "Notifications" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiRequestBody("application/json",typeof(UpdateNotifyDTO))]
        public async Task<HttpResponseData> MarkSingleNotifyAsRead([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext context)
        {
            _logger.LogInformation("MarkSingleNotifyAsRead");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            UpdateNotifyDTO data = JsonConvert.DeserializeObject<UpdateNotifyDTO>(requestBody);
            ResponseModel<string> result = _notificationHandler.MarkSingleNotifyAsRead(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("MarkAllNotifyAsRead")]
        [OpenApiOperation(operationId: "MarkAllNotifyAsRead", tags: new[] { "Notifications" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiRequestBody("application/json", typeof(List<UpdateNotifyDTO>))]
        public async Task<HttpResponseData> MarkAllNotifyAsRead([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext context)
        {
            _logger.LogInformation("MarkAllNotifyAsRead");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            List<UpdateNotifyDTO> data = JsonConvert.DeserializeObject<List<UpdateNotifyDTO>>(requestBody);
            ResponseModel<string> result = _notificationHandler.MarkAllNotifyAsRead(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }
        [Function("GetAllUnreadNotifications")]
        [OpenApiOperation(operationId: "GetAllUnreadNotifications", tags: new[] { "Notifications" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiParameter(name:"companyId", In = ParameterLocation.Query, Required = true, Type = typeof(string))]
        [OpenApiParameter(name: "page", In = ParameterLocation.Query, Required = true, Type = typeof(int))]
        public async Task<HttpResponseData> GetAllUnreadNotifications([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext context)
        {
            _logger.LogInformation("GetAllUnreadNotifications");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            string companyId = queryParam["companyId"].ToString();
            int page = Convert.ToInt32(queryParam["page"]);
            ResponseModel<List<NotificationDTO>> result = _notificationHandler.GetAllUnreadNotifications(companyId,page);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetAllReadNotifications")]
        [OpenApiOperation(operationId: "GetAllReadNotifications", tags: new[] { "Notifications" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiParameter(name: "companyId", In = ParameterLocation.Query, Required = true, Type = typeof(string))]
        [OpenApiParameter(name: "page", In = ParameterLocation.Query, Required = true, Type = typeof(int))]
        public async Task<HttpResponseData> GetAllReadNotifications([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext context)
        {
            _logger.LogInformation("GetAllReadNotifications");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            string companyId = queryParam["companyId"].ToString();
            int page = Convert.ToInt32(queryParam["page"]);
            ResponseModel<List<NotificationDTO>> result = _notificationHandler.GetAllReadNotifications(companyId, page);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }
        [Function("GetNotficationDetailsById")]
        [OpenApiOperation(operationId: "GetNotficationDetailsById", tags: new[] { "Notifications" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiParameter(name: "NotifyId", In = ParameterLocation.Query, Required = true, Type = typeof(string))]
        [OpenApiParameter(name: "CompanyId", In = ParameterLocation.Query, Required = true, Type = typeof(int))]
        public async Task<HttpResponseData> GetNotficationDetailsById([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext context)
        {
            _logger.LogInformation("GetNotficationDetailsById");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            string notifyId = queryParam["NotifyId"].ToString();
            string companyId = queryParam["CompanyId"].ToString();
            ResponseModel<NotificationDTO> result = _notificationHandler.GetNotficationDetailsById(notifyId, companyId);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("SendTestNotification")]
        [OpenApiOperation(operationId: "SendTestNotification", tags: new[] { "Notifications" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiRequestBody("application/json", typeof(TestNotifyDTO))]
        public async Task<HttpResponseData> SendTestNotification([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req, FunctionContext context)
        {
            _logger.LogInformation("SendTestNotification");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            TestNotifyDTO data = JsonConvert.DeserializeObject<TestNotifyDTO>(requestBody);
            ResponseModel<string> result = _notificationHandler.SendTestNotification(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetNotificationsTotal")]
        [OpenApiOperation(operationId: "GetNotificationsTotal", tags: new[] { "Notifications" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiParameter(name: "CompanyId", In = ParameterLocation.Query, Required = true, Type = typeof(int))]
        public async Task<HttpResponseData> GetNotificationsTotal([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext context)
        {
            _logger.LogInformation("GetNotificationsTotal");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            string companyId = queryParam["CompanyId"].ToString();
            ResponseModel<string> result = _notificationHandler.GetNotificationsTotal(companyId);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }
    }
}
