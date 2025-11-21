using System.Net;
using System.Security;
using AutoAzureMob.API.ActionFilter;
using AutoAzureMob.API.Helper;
using AutoAzureMob.BLL.BLL;
using AutoAzureMob.DAL.DAL;
using AutoAzureMob.Models.DTO.DashBoardDTO;
using AutoAzureMob.Models.Models.DashBoard;
using AutoAzureMob.Models.Models.Response;
using AutoAzureMob.Models.VM.DashBoard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using static System.Formats.Asn1.AsnWriter;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AutoAzureMob.API.Functions
{
    [Authorize]
    public class TestFunction
    {
        private readonly ILogger _logger;
        private readonly TestHandler testHandler;
        private readonly IConfiguration config;
        private readonly ExecuteContext context;
        public TestFunction(ILoggerFactory loggerFactory, ExecuteContext _context = null)
        {
            _logger = loggerFactory.CreateLogger<TestFunction>();
            context = _context;
            config = ConfigurationHelper.GetConfiguration();
            testHandler = new TestHandler(context,config);
        }
        [Function("TestFunction")]
        [OpenApiOperation(operationId: "CreateClient", tags: new[] { "Test" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]

        [OpenApiRequestBody("application/json", typeof(TestFunction))]
        public async Task<ActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,FunctionContext _context)
        {
            var userId = _context.Items["userId"];
            string requestBpdy = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBpdy);
            return new JsonResult(testHandler.TestFlow("testing"))
            {
                StatusCode = 200,
                ContentType = "application/json"
            };
        }
        [Function("TestFunction2")]
        [OpenApiOperation(operationId: "TestFunctions", tags: new[] { "Test" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey,Name ="Authorization", In = OpenApiSecurityLocationType.Header,Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiRequestBody("application/json", typeof(TestBody))]

        public ActionResult Run2([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            string requestBpdy =  new StreamReader(req.Body).ReadToEnd();
            TestBody data = JsonConvert.DeserializeObject<TestBody>(requestBpdy);
            return new JsonResult(testHandler.TestFlow("testing"))
            {
                StatusCode = 200,
                ContentType = "application/json"
            };
            throw new Exception();
        }

        [Function("CheckEnvironment")]
        [OpenApiOperation(operationId: "CheckCode", tags: new[] { "Test" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        public async Task<HttpResponseData> CheckEnvironment([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req, FunctionContext _context)
        {
            _logger.LogInformation("CheckEnvironment");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
          
            ResponseModel<string> result = testHandler.CheckCode();
            var response = req.CreateResponse(HttpStatusCode.OK);

            await response.WriteAsJsonAsync(result);

            return response;
        }
        [Function("CancellationTokenTest")]
        [OpenApiOperation(operationId: "CancellationTokenTest", tags: new[] { "Test" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        public async Task<HttpResponseData> CancellationTokenTest([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req, FunctionContext _context,CancellationToken token)
        {
            _logger.LogInformation("CancellationTokenTest running.......");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var response = req.CreateResponse(HttpStatusCode.OK);
            try
            {
                if (token.IsCancellationRequested)
                {
                    return response;
                }
                else
                {
                    for (int i = 0; i < 100; i++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            return response;
                        }
                        await Task.Delay(3000);
                        _logger.LogInformation("Function executing....");
                    }
                    string result = "Testing";


                    await response.WriteAsJsonAsync(result);
                }
                
            }
            catch (Exception)
            {

                throw;
            }
            

            return response;
        }

        public class TestBody
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
