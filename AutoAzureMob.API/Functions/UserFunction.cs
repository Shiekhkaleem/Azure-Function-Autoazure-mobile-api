using System.Net;
using AutoAzureMob.API.ExceptionHandling;
using AutoAzureMob.API.Helper;
using AutoAzureMob.BLL.BLL;
using AutoAzureMob.DAL.DAL;
using AutoAzureMob.Models.DTO.UserDTO;
using AutoAzureMob.Models.Models.Company;
using AutoAzureMob.Models.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AutoAzureMob.API.Functions
{
    public class UserFunction
    {
        private readonly ILogger _logger;
        private readonly UserHandler userHandler;
        private readonly IConfiguration config;
        private readonly ExecuteContext executecontext;
        public UserFunction(ILoggerFactory loggerFactory, ExecuteContext _executecontext = null)
        {
            _logger = loggerFactory.CreateLogger<UserFunction>();
            executecontext = _executecontext;
            config = ConfigurationHelper.GetConfiguration();
            userHandler= new UserHandler(executecontext,config);
        }
        [Function("SignIn")]
        [OpenApiOperation(operationId: "SignIn", tags: new[] { "Sign In" })]
        [OpenApiRequestBody("application/json", typeof(LoginRequest))]
        public async Task<HttpResponseData> SignIn([HttpTrigger(AuthorizationLevel.Anonymous, "post",Route =null)] HttpRequestData req)
        {
            _logger.LogInformation("Sign-In");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            LoginRequest data = JsonConvert.DeserializeObject<LoginRequest>(requestBody);
            var result = userHandler.Login(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

           return response;
        }
        [Function("SignInV2")]
        [OpenApiOperation(operationId: "SignInV2", tags: new[] { "Sign In" })]
        [OpenApiRequestBody("application/json", typeof(LoginRequest))]
        public async Task<HttpResponseData> SignInV2([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("Sign-In");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            LoginRequest data = JsonConvert.DeserializeObject<LoginRequest>(requestBody);
            var result = userHandler.LoginV2(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }
        [Function("Registration")]
        [OpenApiOperation(operationId: "Registration", tags: new[] { "User" })]
        [OpenApiRequestBody("application/json", typeof(RegistRequest))]
        public async Task<HttpResponseData> Registration([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("Registration");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            RegistRequest data = JsonConvert.DeserializeObject<RegistRequest>(requestBody);
          
            var result = userHandler.AdminCompanyRegistration(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }
        [Function("ForgetPasswordRequest")]
        [OpenApiOperation(operationId: "ForgetPasswordRequest", tags: new[] { "User" })]
        [OpenApiRequestBody("application/json", typeof(ForgetPassDTO))]
        public async Task<HttpResponseData> ForgetPasswordRequest([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("Registration");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ForgetPassDTO data = JsonConvert.DeserializeObject<ForgetPassDTO>(requestBody);
            var result = userHandler.ForgetPasswordRequest(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }
    }
}
