using System.Net;
using AutoAzureMob.API.Helper;
using AutoAzureMob.BLL.BLL;
using AutoAzureMob.DAL.DAL;
using AutoAzureMob.Models.DTO.FacturacionDTO;
using AutoAzureMob.Models.DTO.MessagesDTO;
using AutoAzureMob.Models.Models.Facturacion;
using AutoAzureMob.Models.Models.Messages;
using AutoAzureMob.Models.Models.Response;
using AutoAzureMob.Models.Models.User;
using AutoAzureMob.Models.VM.Balance;
using AutoAzureMob.Models.VM.Facturacion;
using AutoAzureMob.Models.VM.Notification;
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
    public class ConfiguracionFunction
    {
        private readonly IConfiguration config;
        private readonly ExecuteContext executecontext;
        private readonly ILogger _logger;
        private readonly ConfiguracionHandler configuracionHandler;

        public ConfiguracionFunction(ILoggerFactory loggerFactory, ExecuteContext _executecontext = null)
        {
            _logger = loggerFactory.CreateLogger<BalanceFunction>();
            executecontext = _executecontext;
            config = ConfigurationHelper.GetConfiguration();
            configuracionHandler = new ConfiguracionHandler(executecontext, config);
        }

        [Function("GetPerfileList")]
        [OpenApiOperation(operationId: "GetPerfileList", tags: new[] { "Configuracion/Facturacion" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiParameter(name: "CompanyId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        [OpenApiParameter(name: "Page", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        public async Task<HttpResponseData> GetPerfileList([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("GetPerfileList");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            int companyId = Convert.ToInt32(queryParam["CompanyId"]);
            int page = Convert.ToInt32(queryParam["Page"]);
            ResponseModel<List<Profile>> result = configuracionHandler.GetPerfileList(companyId,page);
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetUserRelacionList")]
        [OpenApiOperation(operationId: "GetUserRelacionList", tags: new[] { "Configuracion/Facturacion" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiParameter(name: "CompanyId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        public async Task<HttpResponseData> GetUserRelacionList([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("GetUserRelacionList");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            int companyId = Convert.ToInt32(queryParam["CompanyId"]);
            ResponseModel<UserRelacionVM> result = configuracionHandler.GetUserRelacionList(companyId);
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetTimberPerfileList")]
        [OpenApiOperation(operationId: "GetTimberPerfileList", tags: new[] { "Configuracion/Facturacion" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiParameter(name: "CompanyId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        public async Task<HttpResponseData> GetTimberPerfileList([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("GetTimberPerfileList");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            int companyId = Convert.ToInt32(queryParam["CompanyId"]);
            ResponseModel<TimberVM> result = configuracionHandler.GetTimberPerfileList(companyId);
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetTimberQuantity")]
        [OpenApiOperation(operationId: "GetTimberQuantity", tags: new[] { "Configuracion/Facturacion" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiParameter(name: "CompanyId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        [OpenApiParameter(name: "ProfileId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        public async Task<HttpResponseData> GetTimberQuantity([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("GetTimberQuantity");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            string companyId = queryParam["CompanyId"].ToString();
            string profileId = queryParam["ProfileId"].ToString();
            ResponseModel<TimberQuantity> result = configuracionHandler.GetTimberQuantity(companyId,profileId);
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetConfigFormadePagoSetting")]
        [OpenApiOperation(operationId: "GetConfigFormadePagoSetting", tags: new[] { "Configuracion/Facturacion" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiParameter(name: "CompanyId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        public async Task<HttpResponseData> GetConfigFormadePagoSetting([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("GetConfigFormadePagoSetting");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            int companyId = Convert.ToInt32(queryParam["CompanyId"]);
            ResponseModel<ConfiguracionVM> result = configuracionHandler.GetConfigFormadePagoSetting(companyId);
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("SaveUserProfileRelation")]
        [OpenApiOperation(operationId: "SaveUserProfileRelation", tags: new[] { "Configuracion/Facturacion" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiRequestBody("application/json", typeof(List<SaveUserDTO>))]
        public async Task<HttpResponseData> SaveUserProfileRelation([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("SaveUserProfileRelation");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            List<SaveUserDTO> data = JsonConvert.DeserializeObject<List<SaveUserDTO>>(requestBody);
            ResponseModel<string> result = configuracionHandler.SaveUserProfileRelation(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("SaveTimberEmisor")]
        [OpenApiOperation(operationId: "SaveTimberEmisor", tags: new[] { "Configuracion/Facturacion" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiRequestBody("application/json", typeof(TimberRequest))]
        public async Task<HttpResponseData> SaveTimberEmisor([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("SaveTimberEmisor");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            TimberRequest data = JsonConvert.DeserializeObject<TimberRequest>(requestBody);
            ResponseModel<string> result = configuracionHandler.SaveTimberEmisor(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("SaveConfigSetting")]
        [OpenApiOperation(operationId: "SaveConfigSetting", tags: new[] { "Configuracion/Facturacion" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiRequestBody("application/json", typeof(ConfiguracionSetting))]
        public async Task<HttpResponseData> SaveConfigSetting([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("SaveConfigSetting");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ConfiguracionSetting data = JsonConvert.DeserializeObject<ConfiguracionSetting>(requestBody);
            ResponseModel<string> result = configuracionHandler.SaveConfigSetting(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetMainTableUser")]
        [OpenApiOperation(operationId: "GetMainTableUser", tags: new[] { "Configuracion/Usuarios" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiParameter(name: "CompanyId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        [OpenApiParameter(name: "Page", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        public async Task<HttpResponseData> GetMainTableUser([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("GetPerfileList");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            int companyId = Convert.ToInt32(queryParam["CompanyId"]);
            int page = Convert.ToInt32(queryParam["Page"]);
            ResponseModel<List<CompanyUser>> result = configuracionHandler.GetMainTableUser(companyId, page);
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetUserDataById")]
        [OpenApiOperation(operationId: "GetUserDataById", tags: new[] { "Configuracion/Usuarios" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiParameter(name: "CompanyId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        public async Task<HttpResponseData> GetUserDataById([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("GetUserDataById");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            int companyId = Convert.ToInt32(queryParam["CompanyId"]);
            int userId = Convert.ToInt32(queryParam["UserId"]);
            ResponseModel<List<UserData>> result = configuracionHandler.GetUserDataById(companyId, userId);
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("UpdateUserInfo")]
        [OpenApiOperation(operationId: "UpdateUserInfo", tags: new[] { "Configuracion/Usuarios" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiRequestBody("application/json", typeof(UserUpdateDTO))]
        public async Task<HttpResponseData> UpdateUserInfo([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("UpdateUserInfo");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            UserUpdateDTO data = JsonConvert.DeserializeObject<UserUpdateDTO>(requestBody);
            ResponseModel<string> result = configuracionHandler.UpdateUserInfo(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("UpdateUserPassword")]
        [OpenApiOperation(operationId: "UpdateUserPassword", tags: new[] { "Configuracion/Usuarios" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiRequestBody("application/json", typeof(UpdatePasswordDTO))]
        public async Task<HttpResponseData> UpdateUserPassword([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("UpdateUserPassword");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            UpdatePasswordDTO data = JsonConvert.DeserializeObject<UpdatePasswordDTO>(requestBody);
            ResponseModel<string> result = configuracionHandler.UpdateUserPassword(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetPermissionTabs")]
        [OpenApiOperation(operationId: "GetPermissionTabs", tags: new[] { "Configuracion/Usuarios" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiParameter(name: "ModuleId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        [OpenApiParameter(name: "UserId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        public async Task<HttpResponseData> GetPermissionTabs([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("GetPermissionTabs");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            int moduleId = Convert.ToInt32(queryParam["ModuleId"]);
            int userId = Convert.ToInt32(queryParam["UserId"]);
            ResponseModel<List<PermissionTabVM>> result = configuracionHandler.GetPermissionTabs(moduleId, userId);
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("UpdateUserPermission")]
        [OpenApiOperation(operationId: "UpdateUserPermission", tags: new[] { "Configuracion/Usuarios" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiRequestBody("application/json", typeof(List<PermissionDTO>))]
        public async Task<HttpResponseData> UpdateUserPermission([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("UpdateUserPermission");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            List<PermissionDTO> data = JsonConvert.DeserializeObject<List<PermissionDTO>>(requestBody);
            ResponseModel<string> result = configuracionHandler.UpdateUserPermission(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetNotifyPermissionList")]
        [OpenApiOperation(operationId: "GetNotifyPermissionList", tags: new[] { "Configuracion/Notification" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiParameter(name: "CompanyId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        [OpenApiParameter(name: "ChannelId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        public async Task<HttpResponseData> GetNotifyPermissionList([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("GetNotifyPermissionList");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            int companyId = Convert.ToInt32(queryParam["CompanyId"]);
            int channelId = Convert.ToInt32(queryParam["ChannelId"]);
            ResponseModel<List<NotifyPermissionVM>> result = configuracionHandler.GetNotifyPermissionList(companyId, channelId);
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("UpdateNotification")]
        [OpenApiOperation(operationId: "UpdateNotification", tags: new[] { "Configuracion/Notification" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiRequestBody("application/json", typeof(List<NotifyPermissionVM>))]
        public async Task<HttpResponseData> UpdateNotification([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("UpdateNotification");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            List<NotifyPermissionVM> data = JsonConvert.DeserializeObject<List<NotifyPermissionVM>>(requestBody);
            ResponseModel<string> result = configuracionHandler.UpdateNotification(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }
    }
}
