using AutoAzureMob.BLL.Utils;
using AutoAzureMob.Core.AzureBlobServices;
using AutoAzureMob.DAL.DAL;
using AutoAzureMob.Models.DTO.MessagesDTO;
using AutoAzureMob.Models.DTO.QuestionsDTO;
using AutoAzureMob.Models.Models;
using AutoAzureMob.Models.Models.Messages;
using AutoAzureMob.Models.Models.Response;
using AutoAzureMob.Models.VM.MessageVM;
using AutoAzureMob.QueryResource;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureUrl = AutoAzureMob.QueryResource.AzureUrls;
using AzureUrlProd = AutoAzureMob.QueryResource.AzureUrlsProd;

namespace AutoAzureMob.BLL.BLL
{
    public class MessagesHandler  :BaseHandler
    {
        private readonly IConfiguration _config;
        private readonly MessagesDAO _messagesDAO;
        public static string connectionString = string.Empty;
        public static string containerName = string.Empty;
        private static bool LiveServer = false;
        public MessagesHandler(ExecuteContext executeContext, IConfiguration config) : base(executeContext, config)
        {
            _config = config;
            _messagesDAO = new MessagesDAO(executeContext, _config);
            connectionString = config.GetSection("AzureStorage:AzureConnection").Value;
            containerName = config.GetSection("AzureStorage:ContainerName").Value;
            LiveServer = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Production");
        }
        public ResponseModel<List<Message>> GetMessagesList(string userId)
        {
            ResponseModel<List<Message>> response = new ResponseModel<List<Message>>();
            response.Content = _messagesDAO.GetMessagesList(userId);
            response.Description = "Messages list not found.";
            response.Success = true;
            if (response.Content != null && response.Content.Count > 0)
            {
                response.Description = "Messages list";
            }
            return response;
        }
        public ResponseModel<List<Message>> GetMessagesListByFilteration(FilterationDTO req)
        {
            ResponseModel<List<Message>> response = new ResponseModel<List<Message>>();
            response.Content = _messagesDAO.GetMessagesListByFilteration(req);
            response.Description = "Messages list not found.";
            response.Success = true;
            if (response.Content != null && response.Content.Count > 0)
            {
                response.Description = "Messages list";
            }
            return response;
        }
        public ResponseModel<string> SendMessageReply(MessageReplyDTO req)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            List<string> attachmentList = new List<string>();
            AzureUpload azureUpload = new AzureUpload();
            azureUpload.ConnectionString = connectionString;
            azureUpload.Container = containerName;
            
            foreach (var file in req.Attachments)
            {
                Random random = new Random();
                azureUpload.File = file;
                azureUpload.Folder = req.UserMKTId+"_" + random.Next(1000, 10000).ToString()+req.BuyerId;
                string fileUrl = AzureFileUploader.UploadFileToBlob(azureUpload);
                if (fileUrl.Any())
                {
                    string fileName =fileUrl.Split('/').Last();
                    attachmentList.Add(fileName);
                }
            }
            string url = LiveServer ? AzureUrlProd.SendMessageReply : AzureUrl.SendMessageReply;

            string json = JsonConvert.SerializeObject(new
            {
                user_id = req.UserMKTId,
                buyer_id = req.BuyerId,
                order_id = req.OrderId,
                text = req.Text,
                attachments = attachmentList
            });
            AzureResponse result = AzureResponseHandler.GetAzureResponseObject<AzureResponse>(url, json);
            response.Success = result.Success == "true" ? true : false;
            response.Description = result.Description;
            response.Title = result.Title;
            response.Content = result.Content.ToString();
            return response;

        }
        public ResponseModel<MessageVM> GetListMessagingBillingUrl(MessageDTO req)
        {
            ResponseModel<MessageVM> response = new ResponseModel<MessageVM>();
            string url =LiveServer ? AzureUrlProd.GetListMessaging: AzureUrl.GetListMessaging ;

            string json = JsonConvert.SerializeObject(new
            {
                   UserID = req.UserMKTId ?? "",
                   OrderID = req.OrderId ?? ""   
            });
           response = AzureResponseHandler.GetAzureResponseObject<ResponseModel<MessageVM>>(url, json);
            if (response.Content.Messages.Count > 0)
            {
                response.Content.BuyerId = response.Content.Messages[0].BuyerID;
                response.Content.Messages.ForEach(x => { x.IsAdmin = x.UserID == x.FromUserID ? true : false;});
            }
            return response;
        }
    }
}
