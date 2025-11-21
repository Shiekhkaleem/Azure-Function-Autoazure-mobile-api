using System.Net;
using AutoAzureMob.API.Helper;
using AutoAzureMob.BLL.BLL;
using AutoAzureMob.DAL.DAL;
using AutoAzureMob.Models.DTO.QuestionsDTO;
using AutoAzureMob.Models.DTO.UserDTO;
using AutoAzureMob.Models.Models.Questions;
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
    public class QuestionsFunction
    {
        private readonly ILogger _logger;
        private readonly IConfiguration config;
        private readonly ExecuteContext executecontext;
        private readonly QuestionsHandler _questionsHandler;
        public QuestionsFunction(ILoggerFactory loggerFactory, ExecuteContext _executecontext = null)
        {
            _logger = loggerFactory.CreateLogger<QuestionsFunction>();
            executecontext = _executecontext;
            config = ConfigurationHelper.GetConfiguration();
            _questionsHandler = new QuestionsHandler(executecontext, config);
        }

        [Function("GetQuestionsList")]
        [OpenApiOperation(operationId: "GetQuestionsList", tags: new[] { "Questions & Answers" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiRequestBody("application/json", typeof(QuestionDTO))]
        public async Task<HttpResponseData> GetQuestionsList([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("GetQuestionsList");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            QuestionDTO data = JsonConvert.DeserializeObject<QuestionDTO>(requestBody);
            ResponseModel<List<Question>> result = _questionsHandler.GetQuestionsList(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }
        [Function("GetQuestionDetailById")]
        [OpenApiOperation(operationId: "GetQuestionDetailById", tags: new[] { "Questions & Answers" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiRequestBody("application/json", typeof(QAHistoryDTO))]
        public async Task<HttpResponseData> GetQuestionDetailById([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("GetQuestionsList");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            QAHistoryDTO data = JsonConvert.DeserializeObject<QAHistoryDTO>(requestBody);
            ResponseModel<Question> result = _questionsHandler.GetQuestionDetailById(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }
        [Function("GetQuestionAnswerHistory")]
        [OpenApiOperation(operationId: "GetQuestionAnswerHistory", tags: new[] { "Questions & Answers" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiRequestBody("application/json", typeof(QAHistoryDTO))]
        public async Task<HttpResponseData> GetQuestionAnswerHistory([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("GetQuestionsList");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            QAHistoryDTO data = JsonConvert.DeserializeObject<QAHistoryDTO>(requestBody);
            ResponseModel<List<QuestionHistory>> result = _questionsHandler.GetQuestionAnswerHistory(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }
        [Function("GetQuickAnswersList")]
        [OpenApiOperation(operationId: "GetQuickAnswersList", tags: new[] { "Questions & Answers" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "")]
        public async Task<HttpResponseData> GetQuickAnswersList([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("GetQuestionsList");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            string userId = queryParam["userId"];
            ResponseModel<List<QuickAnswer>> result = _questionsHandler.GetQuickAnswersList(userId);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }
        [Function("BlockUserFromQuestioning")]
        [OpenApiOperation(operationId: "BlockUserFromQuestioning", tags: new[] { "Questions & Answers" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiRequestBody("application/json", typeof(BlockRequestDTO))]
        public async Task<HttpResponseData> BlockUserFromQuestioning([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("BlockUserFromQuestioning");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            BlockRequestDTO data = JsonConvert.DeserializeObject<BlockRequestDTO>(requestBody);
            ResponseModel<string> result = _questionsHandler.BlockUserFromQuestioning(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }
        [Function("DeleteQuestionById")]
        [OpenApiOperation(operationId: "DeleteQuestionById", tags: new[] { "Questions & Answers" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiRequestBody("application/json", typeof(QAHistoryDTO))]
        public async Task<HttpResponseData> DeleteQuestionById([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("BlockUserFromQuestioning");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            QAHistoryDTO data = JsonConvert.DeserializeObject<QAHistoryDTO>(requestBody);
            ResponseModel<string> result = _questionsHandler.DeleteQuestionById(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }
        [Function("SendQuestionReply")]
        [OpenApiOperation(operationId: "SendQuestionReply", tags: new[] { "Questions & Answers" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiRequestBody("application/json", typeof(QuestionReplyDTO))]
        public async Task<HttpResponseData> SendQuestionReply([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("BlockUserFromQuestioning");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            QuestionReplyDTO data = JsonConvert.DeserializeObject<QuestionReplyDTO>(requestBody);
            ResponseModel<string> result = _questionsHandler.SendQuestionReply (data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }
    }
}
