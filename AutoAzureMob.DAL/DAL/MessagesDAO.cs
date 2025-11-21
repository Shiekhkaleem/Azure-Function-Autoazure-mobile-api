using AutoAzureMob.Models.DTO.MessagesDTO;
using AutoAzureMob.Models.Models.Messages;
using AutoAzureMob.Models.Models.Questions;
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
    public class MessagesDAO : BaseDAO
    {
        private readonly IConfiguration _config;
        public MessagesDAO(ExecuteContext executeContext, IConfiguration config) : base(executeContext, config)
        {
            _config = config;
        }
        #region Get Mesages List
        public List<Message> GetMessagesList(string userID)
        {
            List<SqlParameter> param = new List<SqlParameter>
            {
                new SqlParameter("@UserID", userID),
            };
            string queryName = "MOB_MKT_MELI_ListOrderMessages";
            List<Message> messages = FetchMessagesList(param, queryName);
            return messages;
        }
        private List<Message> FetchMessagesList(List<SqlParameter> sqlParam, string queryName)
        {
            List<Message> list = null;
            DataSet resultSet = null;
            if (!string.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, sqlParam, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable storesTable = resultSet.Tables[0];
                    if (storesTable.Rows.Count > 0)
                    {
                        list = new List<Message>();
                        foreach (DataRow row in storesTable.Rows)
                        {
                            Message msg = new Message();
                            msg.OrderId = !string.IsNullOrEmpty(row["OrderID"].ToString()) ? Convert.ToInt64(row["OrderID"]) : 0;
                            msg.Title = row["Title"].ToString() ?? "";
                            msg.Received = row["Received"].ToString() ?? "";
                            msg.LastMessage = row["LastMessage"].ToString() ?? "";
                            msg.IDLastMessage = row["IDLastMessage"].ToString() ?? "";
                            msg.Sku = row["Sku"].ToString() ?? "";
                            msg.ReadId = !string.IsNullOrEmpty(row["ReadID"].ToString()) ? Convert.ToInt64(row["ReadID"]) : 0;
                            msg.OrderStatus = !string.IsNullOrEmpty(row["OrderStatus"].ToString()) ? Convert.ToInt32(row["OrderStatus"]) : 0;
                            msg.OrderSubStatus = row["OrderSubStatus"].ToString() ?? "";
                            msg.RowNumber = !string.IsNullOrEmpty(row["RowNumber"].ToString()) ? Convert.ToInt32(row["RowNumber"]) : 0;
                            list.Add(msg);
                        }
                    }
                }
            }
            return list;
        }
        #endregion
        #region Get Messages List By Filteration
        public List<Message> GetMessagesListByFilteration(FilterationDTO req)
        {
            List<SqlParameter> param = new List<SqlParameter>
            {
                new SqlParameter("@UserID", req.UserMKTId),
                new SqlParameter("@StatusID", req.StatusId),
                new SqlParameter("@SubStatus", req.SubStatus),
                new SqlParameter("@FilterBy", req.FilterBy),
                new SqlParameter("@FilterText", req.FilterText),
                new SqlParameter("@Page",req.Page),
                new SqlParameter("@Limit",req.Limit),
            };
            string queryName = "MOB_MKT_MELI_ListOrderMessages";
            List<Message> messages = FetchMessagesListByFilteration(param, queryName);
            return messages;
        }
        private List<Message> FetchMessagesListByFilteration(List<SqlParameter> sqlParam, string queryName)
        {
            List<Message> list = null;
            DataSet resultSet = null;
            if (!string.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, sqlParam, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable storesTable = resultSet.Tables[0];
                    if (storesTable.Rows.Count > 0)
                    {
                        list = new List<Message>();
                        foreach (DataRow row in storesTable.Rows)
                        {
                            Message msg = new Message();
                            msg.OrderId = Convert.ToInt64(row["OrderID"] ?? 0);
                            msg.Title = row["Title"].ToString() ?? "";
                            msg.Received = row["Received"].ToString() ?? "";
                            if (!string.IsNullOrEmpty(msg.Received))
                            {
                                DateTime date = Convert.ToDateTime(msg.Received);
                                DateTime today = DateTime.Now;
                                TimeSpan span = today - date;
                                msg.TotalDays = Math.Round((decimal)span.TotalDays).ToString() + "d";
                            }
                            msg.LastMessage = row["LastMessage"].ToString() ?? "";
                            msg.IDLastMessage = row["IDLastMessage"].ToString() ?? "";
                            msg.Sku = row["Sku"].ToString() ?? "";
                            msg.ReadId = Convert.ToInt64(row["ReadID"] ?? 0);
                            msg.OrderStatus = Convert.ToInt32(row["OrderStatus"] ?? 0);
                            msg.OrderSubStatus = row["OrderSubStatus"].ToString() ?? "";
                            msg.RowNumber = Convert.ToInt32(row["RowNumber"] ?? 0);
                            msg.TotalRows = Convert.ToInt32(row["TotalRows"] ?? 0);
                            list.Add(msg);
                        }
                    }
                }
            }
            return list;
        }
        #endregion
    }
}
