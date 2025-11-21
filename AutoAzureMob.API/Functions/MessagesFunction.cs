using System.IO;
using System.Net;
using AutoAzureMob.API.Helper;
using AutoAzureMob.BLL.BLL;
using AutoAzureMob.DAL.DAL;
using AutoAzureMob.Models.DTO.MessagesDTO;
using AutoAzureMob.Models.DTO.QuestionsDTO;
using AutoAzureMob.Models.Models.Messages;
using AutoAzureMob.Models.Models.Response;
using AutoAzureMob.Models.VM.MessageVM;
using Azure.Core;
using HttpMultipartParser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
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
    public class MessagesFunction
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly MessagesHandler _messagesHandler;
        public MessagesFunction(ILoggerFactory loggerFactory, ExecuteContext executeContext = null)
        {
            _logger = loggerFactory.CreateLogger<MessagesFunction>();
            _config = ConfigurationHelper.GetConfiguration();
            _messagesHandler = new MessagesHandler(executeContext, _config);
        }

        [Function("GetMessagesList")]
        [OpenApiOperation(operationId: "GetMessagesList", tags: new[] { "Messages" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiParameter(name: "userId", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "")]
        public async Task<HttpResponseData> GetMessagesList([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("GetQuestionsList");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            string userId = queryParam["userId"];
            ResponseModel<List<Message>> result = _messagesHandler.GetMessagesList(userId);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }
        [Function("GetMessagesListByFilteration")]
        [OpenApiOperation(operationId: "GetMessagesListByFilteration", tags: new[] { "Messages" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiRequestBody("application/json", typeof(FilterationDTO))]
        public async Task<HttpResponseData> GetMessagesListByFilteration([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("GetQuestionsList");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            FilterationDTO data = JsonConvert.DeserializeObject<FilterationDTO>(requestBody);
            ResponseModel<List<Message>> result = _messagesHandler.GetMessagesListByFilteration(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }
        [Function("GetMessageDetails")]
        [OpenApiOperation(operationId: "GetMessageDetails", tags: new[] { "Messages" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiRequestBody("application/json", typeof(MessageDTO))]
        public async Task<HttpResponseData> GetMessageDetails([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("GetMessageDetails");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            MessageDTO data = JsonConvert.DeserializeObject<MessageDTO>(requestBody);
            ResponseModel<MessageVM> result = _messagesHandler.GetListMessagingBillingUrl(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }
        [Function("SendMessageReply")]
        public async Task<HttpResponseData> SendMessageReply([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("SendMessageReply");
            MessageReplyDTO data = new MessageReplyDTO();
            var requestForm = MultipartFormDataParser.Parse(req.Body);
            data.UserMKTId = requestForm.HasParameter("UserMKTId") ? requestForm.GetParameterValue("UserMKTId") : "";
            data.BuyerId = requestForm.HasParameter("BuyerId") ? requestForm.GetParameterValue("BuyerId") : "";
            data.OrderId = requestForm.HasParameter("OrderId") ? requestForm.GetParameterValue("OrderId") : "";
            data.Text = requestForm.HasParameter("Text") ? requestForm.GetParameterValue("Text") : "";
            for (int i = 0; i < requestForm.Files.Count; i++)
            {
                var file = requestForm.Files[i];
                if (!string.IsNullOrEmpty(file.FileName))
                {
                    Stream fileStream = file.Data;
                    IFormFile formFile = new FormFile(fileStream, 0, fileStream.Length, file.FileName, file.ContentType);
                    data.Attachments.Add(formFile);
                }
            }
            ResponseModel<string> result = _messagesHandler.SendMessageReply(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }
    }
}
