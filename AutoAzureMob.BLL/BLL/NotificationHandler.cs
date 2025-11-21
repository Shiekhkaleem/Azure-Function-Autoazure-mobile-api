using AutoAzureMob.Core.FireBaseServices;
using AutoAzureMob.DAL.DAL;
using AutoAzureMob.Models.DTO.DeviceToken;
using AutoAzureMob.Models.DTO.NotiDTO;
using AutoAzureMob.Models.Models.Response;
using AutoAzureMob.Models.VM.Facturacion;
using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.BLL.BLL
{
    public class NotificationHandler : BaseHandler
    {
        private readonly NotificationDAO notificationDAO;
        private readonly UserDAO userDAO;
        private readonly IConfiguration config;
        public NotificationHandler(ExecuteContext executeContext, IConfiguration _config) : base(executeContext, _config)
        {
            config = _config;
            notificationDAO = new NotificationDAO(executeContext, config);
            userDAO = new UserDAO(executeContext, config);
        }
        public ResponseModel<string> RemoveUserDeviceToken(int userId, string type)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            var result = userDAO.DeleteUserDeviceToken(userId, type);
            response.Description = "An Error occurred while Deleting Device Token.";
            if (!string.IsNullOrEmpty(result))
            {
                response.Success = true;
                response.Description = "Device token removed successfully.";
            }
            return response;
        }
        public List<string> GetUserDeviceTokensById(int userId)
        {
            List<string> response = new List<string>();
            response = notificationDAO.GetUserDeviceTokensById(userId);
            return response;
        }
        public ResponseModel<string> MarkSingleNotifyAsRead(UpdateNotifyDTO req)
        {
            ResponseModel<string> response = new();
            response.Success = false;
            response.Title = "Fallida";
            response.Content = notificationDAO.MarkAs("read", req.CompanyId, req.NotifyId);
            if (response.Content.Equals("ok"))
            {
                response.Success = true;
                response.Title = "Éxito";
            }
            return response;
        }
        public ResponseModel<string> MarkAllNotifyAsRead(List<UpdateNotifyDTO> req)
        {
            ResponseModel<string> response = new();
            response.Success = false;
            response.Title = "Fallida";
            response.Description = "Notificaciones no marcadas, algo salió mal.";
            foreach (var item in req)
            {
                response.Content = notificationDAO.MarkAs("read", item.CompanyId, item.NotifyId);
                if (response.Content.Equals("ok"))
                {
                    response.Success = true;
                    response.Description = "Toda la notificación ha sido leída";
                    response.Title = "Éxito";
                }
            }
            return response;
        }
        public ResponseModel<List<NotificationDTO>> GetAllUnreadNotifications(string companyId, int page)
        {
            ResponseModel<List<NotificationDTO>> response = new();
            response.Content = new();
            response.Success = true;
            int total = 0;
            response.Description = "Notifications not found";
            //List<NotificationDTO> notificationsAA = notificationDAO.GetMobNotifications(companyId, "1", 1, page);
            //if (notificationsAA.Count > 0)
            //{
            //    response.Content.AddRange(notificationsAA);
            //    total = Convert.ToInt32(notificationsAA.FirstOrDefault().Total);
            //}
            List<NotificationDTO> notificatinosMASS = notificationDAO.GetMobNotifications(companyId, "2", 1, page);
            if (notificatinosMASS.Count > 0)
            {
                response.Content.AddRange(notificatinosMASS);
                total += Convert.ToInt32(notificatinosMASS.FirstOrDefault().Total);
            }
            if (response.Content.Count > 0)
            {
                response.Content = response.Content.OrderByDescending(X => X.DateCreated).ToList();
                response.Content.ForEach(x => { x.Total = total.ToString(); });
                response.Description = "All notifications list";
            }
            return response;
        }
        public ResponseModel<List<NotificationDTO>> GetAllReadNotifications(string companyId, int page)
        {
            ResponseModel<List<NotificationDTO>> response = new();
            response.Content = new();
            response.Success = true;
            response.Content = notificationDAO.GetReadNotifications(companyId, page);
            response.Description = "Notifications not found";
            if (response.Content.Count > 0)
            {
                response.Content = response.Content.OrderByDescending(X => X.DateCreated).ToList();
                response.Description = "All notifications list";
            }
            return response;
        }
        public ResponseModel<List<NotificationDTO>> GetAllNewNotifications(string companyId, int type)
        {
            ResponseModel<List<NotificationDTO>> response = new();
            response.Content = new();
            response.Content = notificationDAO.GetNewMOBNotifications(companyId, type);
            response.Description = "Notifications not found";
            if (response.Content.Count > 0)
            {
                response.Success = true;
                response.Description = "All notifications list";
            }
            return response;
        }
        public ResponseModel<NotificationDTO> GetNotficationDetailsById(string notifyId, string companyId)
        {
            ResponseModel<NotificationDTO> response = new();
            response.Content = new();
            response.Content = notificationDAO.GetNotificationDetail(notifyId, companyId);
            response.Description = "Notification not found";
            if (response.Content != null)
            {
                response.Success = true;
                response.Description = "Notifications";
            }
            return response;
        }
        public ResponseModel<string> SendTestNotification(TestNotifyDTO req)
        {
            ResponseModel<string> response = new();
            SendNotifyDTO sendNotify = new SendNotifyDTO()
            {
                NotifyId = "-849368T38",
                Total = "12",
                CompanyId = "999032",
            };
            Notification notification = new Notification()
            {
                Title = req.Title,
                Body = req.Body
            };
            NotificationService.SendNotification(req.DeviceToken, notification, sendNotify);
            response.Title = "Send";
            response.Success = true;
            response.Description = "Test notification has been sent.";
            return response;
        }

        #region Send Trigger Notification
        public void SendTriggerNewNotifications()
        {
            List<UserDeviceDTO> userTokens = notificationDAO.GetAllUserDeviceTokens();
            var userGroup = userTokens.GroupBy(x => x.CompanyId);

            if (userTokens != null && userTokens.Any())
            {

                foreach (var user in userGroup)
                {
                    ResponseModel<string> totalResponse = GetNotificationsTotal(user.Key);
                    //Type 1 Notification
                    List<NotificationDTO> newNotifications = notificationDAO.GetNewMOBNotifications(user.Key, 1);
                    foreach (var notify in newNotifications)
                    {
                        foreach (var group in user)
                        {
                            SendNotifyDTO sendNotify = new SendNotifyDTO()
                            {
                                NotifyId = notify.ID,
                                Total = totalResponse.Content,
                                CompanyId = group.CompanyId,
                            };
                            Notification notification = new Notification()
                            {
                                Title = notify.Title,
                                Body = notify.Message,
                                ImageUrl = !string.IsNullOrEmpty(notify.Picture) ? notify.Picture.Replace("http://", "https://") : notify.Logo
                            };
                            NotificationService.SendNotification(group.DeviceToken, notification, sendNotify);
                        }
                        notificationDAO.MarkAsReceived(user.Key, new List<NotificationDTO> { notify });
                    }

                    //Type 2 Notification
                    List<NotificationDTO> newNotificationsTypeTwo = notificationDAO.GetNewMOBNotifications(user.Key, 2);
                    if (newNotificationsTypeTwo != null)
                    {
                        foreach (var notifyItem in newNotificationsTypeTwo)
                        {
                            var userToken = userTokens.FirstOrDefault(x => x.CompanyId == notifyItem.CompanyID);
                            if (user != null)
                            {
                                SendNotifyDTO sendNotify = new SendNotifyDTO()
                                {
                                    NotifyId = notifyItem.ID,
                                    Total = totalResponse.Content,
                                    CompanyId = userToken.CompanyId,
                                };
                                Notification notification = new Notification()
                                {
                                    Title = notifyItem.Title,
                                    Body = notifyItem.Message,
                                    ImageUrl = !string.IsNullOrEmpty(notifyItem.Picture) ? notifyItem.Picture.Replace("http://", "https://") : notifyItem.Logo
                                };
                                NotificationService.SendNotification(userToken.DeviceToken, notification, sendNotify);
                            }

                            notificationDAO.MarkAsReceived(userToken.CompanyId, new List<NotificationDTO> { notifyItem });
                        }
                    }
                }

            }
        }
        #endregion

        public ResponseModel<string> GetNotificationsTotal(string companyId)
        {
            ResponseModel<string> response = new();
            int total = 0;
            response.Description = "Notifications not found";
            List<NotificationDTO> notificationsAA = notificationDAO.GetMobNotifications(companyId, "1", 1, 1);
            response.Success = true;
            //if (notificationsAA.Count > 0)
            //{
            //    total = Convert.ToInt32(notificationsAA.FirstOrDefault().Total);
            //}
            List<NotificationDTO> notificatinosMASS = notificationDAO.GetMobNotifications(companyId, "2", 1, 1);
            if (notificatinosMASS.Count > 0)
            {
                total += Convert.ToInt32(notificatinosMASS.FirstOrDefault().Total);
            }
            response.Content = total.ToString();
            response.Description = "Total count of notifications";
            return response;
        }
    }
}
