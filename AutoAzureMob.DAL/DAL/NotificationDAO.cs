using AutoAzureMob.Models.DTO.DeviceToken;
using AutoAzureMob.Models.DTO.NotiDTO;
using AutoAzureMob.Models.Models.Response;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.DAL.DAL
{
    public class NotificationDAO : BaseDAO
    {
        private readonly IConfiguration config;
        private readonly string notificationString;
        public NotificationDAO(ExecuteContext executionContext, IConfiguration _config) : base(executionContext, _config)
        {
            config = _config;
            notificationString = config.GetConnectionString("AutoAzure-NOTI");

        }
        #region Save Device Token
        public string SaveUserDeviceToken(DeviceTokenDTO device)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
               new SqlParameter("@UserId",device.UserId),
               new SqlParameter("@Token",device.DeviceToken),
               new SqlParameter("@Type",device.Type),
            };                                            
            string queryName = "MOB_NOTIFY_SaveUserDeviceToken";
            string response = ExecuteNonQuery(ExecutionContext, queryName, param, true).ToString();
            return response;
        }
        #endregion
        #region Get User Device Tokens
        public List<string> GetUserDeviceTokensById(int userId)
        {
            List<string> response = new List<string>();
            List<SqlParameter> param = new List<SqlParameter>()
            {
             new SqlParameter("@UserId",userId)
            };
            string queryName = "NOTIFY_GetDeviceTokensById";
            response = FetchDeviceTokensResponse(queryName, param);
            return response ?? new List<string>();
        }
        public List<string> FetchDeviceTokensResponse(string queryName, List<SqlParameter> param)
        {
            List<string> list = new List<string>(); ;
            if (!String.IsNullOrWhiteSpace(queryName))
            {
                DataSet resultSet = ExecuteAdapter(queryName, param, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable Table = resultSet.Tables[0];
                    if (Table.Rows.Count > 0)
                    {
                        list = new List<string>();
                        foreach (DataRow row in Table.Rows)
                        {
                            string token = !string.IsNullOrEmpty(row["DeviceToken"].ToString()) ? row["DeviceToken"].ToString() : "";
                            list.Add(token);
                        }
                    }
                }
            }
            return list;
        }
        #endregion
        #region Get All Users Device Tokens
        public List<UserDeviceDTO> GetAllUserDeviceTokens()
        {
            List<UserDeviceDTO> response = new List<UserDeviceDTO>();
            List<SqlParameter> param = new List<SqlParameter>()
            {
            };
            string queryName = "MOB_NOTIFY_GetAllDeviceTokens";
            response = FetchAllUserDeviceTokens(queryName, param);
            return response ?? new List<UserDeviceDTO>();
        }
        private List<UserDeviceDTO> FetchAllUserDeviceTokens(string queryName, List<SqlParameter> param)
        {
            List<UserDeviceDTO> list = new List<UserDeviceDTO>(); ;
            if (!String.IsNullOrWhiteSpace(queryName))
            {
                DataSet resultSet = ExecuteAdapter(queryName, param, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable Table = resultSet.Tables[0];
                    if (Table.Rows.Count > 0)
                    {
                        list = new List<UserDeviceDTO>();
                        foreach (DataRow row in Table.Rows)
                        {
                            UserDeviceDTO token = new();
                            token.CompanyId = row["UserId"].ToString()?? string.Empty;
                            token.DeviceToken = row["DeviceToken"].ToString()?? string.Empty;
                            list.Add(token);
                        }
                    }
                }
            }
            return list;
        }
        #endregion
        #region Autoazure Notifications
        public List<NotificationDTO> GetMobNotifications(string companyID, string type, int status, int page = 1)
        {
            List<NotificationDTO> notifications = new List<NotificationDTO>();
            try
            {
                using (SqlConnection cnn = new SqlConnection(notificationString))
                {
                    SqlCommand cmd = new SqlCommand("exec MOB_NOT_GetNotifications @val1, @val2, @val3, @val4, @val5", cnn); //-TEMP_NOT_getNotificationsByCompany
                    cmd.Parameters.Clear();
                    //cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@val1", companyID);
                    cmd.Parameters.AddWithValue("@val2", int.Parse(type));
                    cmd.Parameters.AddWithValue("@val3", status);
                    cmd.Parameters.AddWithValue("@val4", 1);
                    cmd.Parameters.AddWithValue("@val5", page);
                    cnn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows == true)
                    {
                        while (dr.Read())
                        {
                            NotificationDTO notification = new NotificationDTO();
                            notification.ID = dr["ID"].ToString();
                            notification.Type = dr["Type"].ToString();
                            notification.CatalogID = dr["CatalogID"].ToString();
                            notification.DateCreated = dr["DateCreated"].ToString();
                            notification.Channel = dr["Channel"].ToString();
                            notification.Logo = dr["Logo"].ToString();
                            notification.Color = dr["Color"].ToString();
                            notification.AccountID = dr["AccountID"].ToString();
                            notification.NickName = dr["NickName"].ToString();
                            notification.Title = dr["Title"].ToString();
                            notification.Body = dr["Body"].ToString();
                            notification.Message = dr["Message"].ToString();
                            notification.Picture = dr["Picture"].ToString();
                            notification.Status = dr["Status"].ToString();
                            notification.ResourceID = dr["ResourceID"].ToString();
                            notification.ResourceName = dr["ResourceName"].ToString();
                            notification.CompanyID = dr["CompanyID"].ToString();
                            notification.Total = dr["Total"].ToString();
                            notifications.Add(notification);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return notifications;
        }

        public List<NotificationDTO> GetNewMOBNotifications(string companyID, int type)
        {
            List<NotificationDTO> notifications = new List<NotificationDTO>();
            try
            {
                using (SqlConnection cnn = new SqlConnection(notificationString))
                {
                    SqlCommand cmd = new SqlCommand("exec MOB_NOT_GetNewNotifications @val1, @val2", cnn); 
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@val1", companyID);
                    cmd.Parameters.AddWithValue("@val2", type);
                    cnn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows == true)
                    {
                        while (dr.Read())
                        {
                            NotificationDTO notification = new NotificationDTO();
                            notification.ID = dr["ID"].ToString();
                            notification.Type = dr["Type"].ToString();
                            notification.CatalogID = dr["CatalogID"].ToString();
                            notification.DateCreated = dr["DateCreated"].ToString();
                            notification.Channel = dr["Channel"].ToString();
                            notification.AccountID = dr["AccountID"].ToString();
                            notification.NickName = dr["NickName"].ToString();
                            notification.Title = dr["Title"].ToString();
                            notification.Body = dr["Body"].ToString();
                            notification.Message = dr["Message"].ToString();
                            notification.Picture = dr["Picture"].ToString();
                            notification.Logo = dr["Logo"].ToString();
                            notification.Status = dr["Status"].ToString();
                            notification.ResourceID = dr["ResourceID"].ToString();
                            notification.ResourceName = dr["ResourceName"].ToString();
                            notification.CompanyID = dr["CompanyID"].ToString();
                            notifications.Add(notification);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return notifications;
        }
        public List<NotificationDTO> GetReadNotifications(string companyID, int page = 1)
        {
            List<NotificationDTO> notifications = new List<NotificationDTO>();
            try
            {
                //var cnn = new DB().con;
                using (SqlConnection cnn = new SqlConnection(notificationString))
                {
                    SqlCommand cmd = new SqlCommand("exec MOB_NOT_GetReadNotifications @val1, @val2, @val3", cnn); //-TEMP_NOT_getNotificationsByCompany
                    cmd.Parameters.Clear();
                    //cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@val1", companyID);
                    cmd.Parameters.AddWithValue("@val2", 1);
                    cmd.Parameters.AddWithValue("@val3", page);
                    cnn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows == true)
                    {
                        while (dr.Read())
                        {
                            NotificationDTO notification = new NotificationDTO();
                            notification.ID = dr["ID"].ToString();
                            notification.Type = dr["Type"].ToString();
                            notification.CatalogID = dr["CatalogID"].ToString();
                            notification.DateCreated = dr["DateCreated"].ToString();
                            notification.Channel = dr["Channel"].ToString();
                            notification.Logo = dr["Logo"].ToString();
                            notification.Color = dr["Color"].ToString();
                            notification.AccountID = dr["AccountID"].ToString();
                            notification.NickName = dr["NickName"].ToString();
                            notification.Title = dr["Title"].ToString();
                            notification.Body = dr["Body"].ToString();
                            notification.Message = dr["Message"].ToString();
                            notification.Picture = dr["Picture"].ToString();
                            notification.Status = dr["Status"].ToString();
                            notification.ResourceID = dr["ResourceID"].ToString();
                            notification.ResourceName = dr["ResourceName"].ToString();
                            notification.CompanyID = dr["CompanyID"].ToString();
                            notification.Total = dr["Total"].ToString();
                            notifications.Add(notification);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return notifications;
        }
        public string MarkAsReceived(string companyID, List<NotificationDTO> notifications)
        {
            try
            {
                /*using (SqlConnection cnn = new SqlConnection(new Connection().ConnectionName))
                {
                    foreach (var n in notifications)
                    {
                        SqlCommand cmd = new SqlCommand("update NOT_Notifications set [DateNotified] = dateadd(hour, -5, getdate()) where ID = @val1 and CompanyID = @val2", cnn); //-TEMP_NOT_Notifications
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@val1", n.ID);
                        cmd.Parameters.AddWithValue("@val2", companyID);
                        cnn.Open();
                        cmd.ExecuteScalar();
                        cnn.Close();
                    }
                }*/
                using (SqlConnection cnn = new SqlConnection(notificationString))
                {
                    cnn.Open();
                    foreach (var _notification in notifications)
                    {
                        SqlCommand cmd = new SqlCommand("exec NOT_MarkNotificationAs @val1, @val2, @val3", cnn);
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@val1", "received");
                        cmd.Parameters.AddWithValue("@val2", Convert.ToInt64(_notification.ID));
                        cmd.Parameters.AddWithValue("@val3", int.Parse(companyID));
                        //cnn.Open();
                        cmd.ExecuteScalar();
                        //cnn.Close();
                    }
                    cnn.Close();
                }

                return "ok";
            }
            catch (Exception ex)
            {
                RegisterException("SP-MarkAsReceived", "Company: " + companyID + "|Message: " + ex.ToString());
                return "error";
            }
        }

        public string MarkAsRead(string id, string companyID)
        {
            try
            {
                string success = "", description = "";
                using (SqlConnection cnn = new SqlConnection(notificationString))
                {                                   //update TEMP_NOT_Notifications set DateRead = dateadd(hour, -6, getdate()), [Status] = 0 where ID = @val1 and CompanyID = @val2
                    SqlCommand cmd = new SqlCommand("exec NOT_MarkNotificationAsRead @val1, @val2", cnn);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@val1", Convert.ToInt64(id));
                    cmd.Parameters.AddWithValue("@val2", int.Parse(companyID));
                    //cnn.Open();
                    //cmd.ExecuteScalar();
                    //cnn.Close();
                    cnn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows == true)
                    {
                        while (dr.Read())
                        {
                            success = dr["success"].ToString();
                            description = dr["message"].ToString();
                        }
                    }
                    cnn.Close();
                }

                if (success != "ok")
                    RegisterException("SP-MarkAsRead", "NotID: " + id + ", Company: " + companyID + "|SPError: " + description);

                return "ok";
            }
            catch (Exception ex)
            {
                RegisterException("SP-MarkAsRead", "NotID: " + id + ", Company: " + companyID + "|Excp: " + ex.ToString());
                return "error";
            }
        }

        public string MarkAs(string status, string companyID, string notificationID)
        {
            try
            {
                string success = "", description = "";
                using (SqlConnection cnn = new SqlConnection(notificationString))
                {
                    SqlCommand cmd = new SqlCommand("exec NOT_MarkNotificationAs @val1, @val2, @val3", cnn);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@val1", status);
                    cmd.Parameters.AddWithValue("@val2", Convert.ToInt64(notificationID));
                    cmd.Parameters.AddWithValue("@val3", int.Parse(companyID));
                    cnn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows == true)
                    {
                        while (dr.Read())
                        {
                            success = dr["success"].ToString();
                            description = dr["message"].ToString();
                        }
                    }
                    cnn.Close();
                }
                    if (success != "ok")
                        RegisterException("SP-MarkAs", "Status: " + status + ", ID: " + notificationID + "| SPError: " + description);

                return "ok";
            }
            catch (Exception ex)
            {
                RegisterException("SP-MarkAs", "Status: " + status + " |Excp: " + ex.ToString());
                return "error";
            }
        }

        public void RegisterException(string taskName, string message)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(notificationString))
                {
                    SqlCommand cmd = new SqlCommand("exec NOT_InsertException @val1, @val2", cnn);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@val1", taskName);
                    cmd.Parameters.AddWithValue("@val2", message);
                    cnn.Open();
                    cmd.ExecuteScalar();
                    cnn.Close();
                }
            }
            catch (Exception ex)
            {
                SqlConnection cnn = new SqlConnection(notificationString);
                SqlCommand cmd = new SqlCommand("insert into NOT_Notifications_Exceptions(TaskName, [Message], [Date]) values( @val1, @val2, dateadd(hour, -5, getdate()) )", cnn);
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@val1", taskName);
                cmd.Parameters.AddWithValue("@val2", "Error al guardar ErrorMsg excepcion: " + ex.ToString() + ". Message: " + message);
                cnn.Open();
                cmd.ExecuteScalar();
                cnn.Close();
            }
        }
        public  NotificationDTO GetNotificationDetail(string id, string companyID = null)
        {
            NotificationDTO notification = new NotificationDTO();
            try
            {
                using (SqlConnection cnn = new SqlConnection(notificationString))
                {
                    SqlCommand cmd = new SqlCommand("exec MOB_NOT_GetNotificationDetail @val1, @val2", cnn);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@val1", Convert.ToInt64(id));
                    cmd.Parameters.AddWithValue("@val2", string.Empty);
                    cnn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows == true)
                    {
                        while (dr.Read())
                        {
                            notification.ID = dr["ID"].ToString();
                            notification.Type = dr["Type"].ToString();
                            notification.CatalogID = dr["CatalogID"].ToString();
                            notification.DateCreated = dr["DateCreated"].ToString();
                            notification.Channel = dr["Channel"].ToString();
                            notification.Logo = dr["Logo"].ToString();
                            notification.AccountID = dr["AccountID"].ToString();
                            notification.NickName = dr["NickName"].ToString();
                            notification.Title = dr["Title"].ToString();
                            notification.Body = dr["Body"].ToString();
                            notification.Message = dr["Message"].ToString();
                            notification.Picture = dr["Picture"].ToString();
                            notification.Status = dr["Status"].ToString();
                            notification.ResourceID = dr["ResourceID"].ToString();
                            notification.ResourceName = dr["ResourceName"].ToString();
                            notification.CompanyID = dr["CompanyID"].ToString();
                        }
                    }
                }
                //cnn.Close();
            }
            catch (Exception ex)
            {
                ///guardar excepcion
                RegisterException("SP-GetNotificationDetail", "Company: " + companyID + ", NotID: " + id + "|Excp: " + ex.ToString());
            }
            return notification;
        }

        #endregion
    }
}
