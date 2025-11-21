using AutoAzureMob.BLL.Utils;
using AutoAzureMob.DAL.DAL;
using AutoAzureMob.Models.DTO.QuestionsDTO;
using AutoAzureMob.Models.Models;
using AutoAzureMob.Models.Models.Questions;
using AutoAzureMob.Models.Models.Response;
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
    public class QuestionsHandler :BaseHandler
    {
        private readonly IConfiguration _config;
        private readonly QuestionsDAO _questionsDAO;
        private static bool LiveServer = false;
        public QuestionsHandler(ExecuteContext executeContext, IConfiguration config) : base(executeContext, config)
        {
            _config = config;
            _questionsDAO = new QuestionsDAO(executeContext, _config);
            LiveServer = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Production");
        }
        public ResponseModel<List<Question>> GetQuestionsList(QuestionDTO req)
        {
            ResponseModel<List<Question>> response = new ResponseModel<List<Question>>();
            response.Content = _questionsDAO.GetQuestionsByStatus(req);
            response.Success = true;
            response.Description = "No hay datos contra el mercado seleccionado.";
            if (response.Content !=null && response.Content.Count > 0)
            {
                response.Description = "List of questions";
            }
            return response;
        }
        public ResponseModel<Question> GetQuestionDetailById(QAHistoryDTO req)
        {
            ResponseModel<Question> response = new ResponseModel<Question>();
            response.Content = _questionsDAO.GetQuestionDetailById(req);
            response.Success = true;
            response.Description = "Detalles de la pregunta no encontrados.";
            if (response.Content != null)
            {
                response.Description = "Detail of question";
            }
            return response;
        }
        public ResponseModel<List<QuestionHistory>> GetQuestionAnswerHistory(QAHistoryDTO req)
        {
            ResponseModel<List<QuestionHistory>> response = new ResponseModel<List<QuestionHistory>>();
            var questionHistories = _questionsDAO.GetQuestionAnswerHistory(req);
                                 
            response.Success = true;
            response.Description = "Historial de preguntas y respuestas no encontrado.";
            if (questionHistories != null && questionHistories.Count > 0)
            {
                response.Content = questionHistories.Select(x => new QuestionHistory
                {
                   ID = x.ID,
                   Message = x.Message,
                   Date = x.Date,
                   UserMKTId = x.UserMKTId,
                   IsAdmin = req.UserMKTId==x.UserMKTId ? true : false,
                }).ToList();
                response.Description = "List of questions and answers";
            }
            return response;
        }
        public ResponseModel<List<QuickAnswer>> GetQuickAnswersList(string userMKTId)
        {
            ResponseModel<List<QuickAnswer>> response = new ResponseModel<List<QuickAnswer>>();
            response.Content = _questionsDAO.GetQuickAnswersList(userMKTId);
            response.Success = true;
            response.Description = "Lista de respuestas rápidas no encontrada.";
            if (response.Content != null && response.Content.Count > 0)
            {
                response.Description = "List of quick answers.";
            }
            return response;
        }
        public ResponseModel<string> BlockUserFromQuestioning(BlockRequestDTO req)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            string URL = LiveServer ? AzureUrlProd.MKT_MELI_QuestionBlacklist : AzureUrl.MKT_MELI_QuestionBlacklist;
            string url = URL + "&UserID=" + req.UserMKTId + "&BlockedUserID=" + req.BlockUserId + "&Type=" + req.Type; 
            //string url = AzureUrl.BlockUserFromQuestioning.Replace("{Type}",req.Type).Replace("{UserId}",req.UserMKTId).Replace("{BlockUserId}",req.BlockUserId);
            string json = JsonConvert.SerializeObject(new { 
            });
            var result = AzureResponseHandler.GetAzureResponseObject<string>(url,json);
            AzureResponse data = JsonConvert.DeserializeObject<AzureResponse>(result);
            response.Description ="El usuario ya está bloqueado para hacer preguntas.";
            if (!data.Status.Contains("already"))
            {
                response.Success = true;
                response.Description = "La usuario ha sido bloqueada.";
            }
           
            return response;

        }
        public ResponseModel<string> DeleteQuestionById(QAHistoryDTO req)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            string URL = LiveServer ? AzureUrlProd.DeleteQuestionById : AzureUrl.DeleteQuestionById;
            string url = URL +"&UserID="+req.UserMKTId+ "&QuestionID=" + req.QuestionId;
            string json = JsonConvert.SerializeObject(new
            {
            });
            var result = AzureResponseHandler.GetAzureResponseObject<string>(url, json);
            AzureResponse data = JsonConvert.DeserializeObject<AzureResponse>(result);
            response.Success = true;
            response.Description = "La pregunta ha sido eliminada";
            response.Content = data.Message;
            if (data.Status != "200")
            {
                response.ExceptionMessage = "Error! "+data.Error +" "+ data.Message;
                response.Success = false;
                response.Description = "Esta pregunta debe quedar sin respuesta.";
                response.Content = "";
            }

            return response;

        }
        public ResponseModel<string> SendQuestionReply(QuestionReplyDTO replyDTO)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            string URL = LiveServer ? AzureUrlProd.PostQuestionReply : AzureUrl.PostQuestionReply;
            string url = URL + "&QuestionID=" + replyDTO.QuestionId + "&AnsweredBy=" + replyDTO.AnsweredBy + "&Text=" + replyDTO.Text;
            string json = JsonConvert.SerializeObject(new { });
            response.Content = AzureResponseHandler.GetAzureResponseObject<string>(url, json);
            response.Description = "Esta pregunta del usuario ya ha sido respondida";
            response.Title = "Error!";
            if (!response.Content.Contains("already"))
            {
                response.Success = true;
                response.Title = "Enviada!";
                response.Description = "¡Pregunta respondida y respuesta guardada!";
            }
            return response;
        }
    }
}
