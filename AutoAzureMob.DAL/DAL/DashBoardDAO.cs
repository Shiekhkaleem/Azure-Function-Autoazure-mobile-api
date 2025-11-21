using AutoAzureMob.Models.DTO.DashBoardDTO;
using AutoAzureMob.Models.DTO.UserDTO;
using AutoAzureMob.Models.Models.DashBoard;
using AutoAzureMob.Models.Models.User;
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
    public class DashBoardDAO : BaseDAO
    {
        private readonly IConfiguration config;
        public DashBoardDAO(ExecuteContext executionContext, IConfiguration _config) : base(executionContext, _config)
        {
            config = _config;
        }
        #region Get Dashboard Data
        public DashboardData GetDashboardData(DashBoardRequestDTO req)
        {
            List<SqlParameter> param = new List<SqlParameter>()
             {
                new  SqlParameter("@ChannelID",req.ChannelId),
                new  SqlParameter("@UserID",req.UserMKTID),
                new  SqlParameter("@FromDate",req.FromDate),
                new  SqlParameter("@ToDate",req.ToDate),
             };
            string queryName = "MOB_DASH_GetStatsData";
            DashboardData response = FetchDashBoardData(queryName,param);
            return response;
        }
        private DashboardData FetchDashBoardData(string queryName, List<SqlParameter> param)
        {
            DashboardData data = new DashboardData();
            DataSet resultSet = null;
            if (!String.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, param, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable Table = resultSet.Tables[0];
                    if (Table.Rows.Count > 0)
                    {
                        foreach (DataRow row in Table.Rows)
                        {
                            data.Visits = !string.Equals(row["Visits"].ToString(), "0") ? Convert.ToDecimal(row["Orders"]).ToString("#,###.##") : "0";
                            data.Orders = !string.Equals(row["Orders"].ToString(),"0") ? Convert.ToDecimal(row["Orders"]).ToString("#,###.##") : "0";
                            data.Shipments = !string.Equals(row["Shipments"].ToString(), "0") ? Convert.ToDecimal(row["Shipments"]).ToString("#,###.##") : "0";
                            data.DelayedShipments = !string.Equals(row["DelayedShipments"].ToString(), "0") ? Convert.ToDecimal(row["DelayedShipments"]).ToString("#,###.##") : "0";
                            data.Claims = !string.Equals(row["Claims"].ToString(), "0") ? Convert.ToDecimal(row["Claims"]).ToString("#,###.##") : "0";
                            data.Questions = !string.Equals(row["Questions"].ToString(), "0") ? Convert.ToDecimal(row["Questions"]).ToString("#,###.##") : "0";
                            data.Payments = !string.Equals(row["Payments"].ToString(),"0") ? Convert.ToDecimal(row["Payments"]).ToString("#,###.##") : "0";
                            data.AveragePayments = !string.Equals(row["AveragePayments"].ToString(),"0") ? Convert.ToDecimal(row["AveragePayments"]).ToString("#,###.##") : "0";
                            data.Accounts = !string.Equals(row["Accounts"].ToString(),"0") ? Convert.ToDecimal(row["Accounts"]).ToString("#,###.##") : "0";
                        }

                    }
                }

            }
            return data;
        }
        #endregion
        #region Get Linked Accounts
        public List<LinkedAccounts> GetLinkedAccounts(AccountsDTO req)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
               new SqlParameter("@CompanyID",req.CompanyId),
               //new SqlParameter("@ChannelID",req.ChannelId == 0? (object)DBNull.Value: req.ChannelId)
            };
            string queryName = "MOB_DASH_GetLinkedAccounts";
            List<LinkedAccounts> response = FetchLinkedAccountsList(queryName, param);
            return response;
        }
        private List<LinkedAccounts> FetchLinkedAccountsList(string queryName, List<SqlParameter> param)
        {
            List<LinkedAccounts> list = new List<LinkedAccounts>();
            DataSet resultSet = null;
            if (!String.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, param, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable Table = resultSet.Tables[0];
                    if (Table.Rows.Count > 0)
                    {
                        foreach (DataRow row in Table.Rows)
                        {
                            LinkedAccounts account = new LinkedAccounts();
                            account.CompanyId = !string.IsNullOrEmpty(row["CompanyID"].ToString())? Convert.ToInt32(row["CompanyID"]) : 0;
                            account.UserMKTId = row["UserID"].ToString() ?? "";
                            account.StoreName = row["StoreName"].ToString() ?? "";
                            account.ChannelId = !string.IsNullOrEmpty(row["ChannelID"].ToString()) ? Convert.ToInt32(row["ChannelID"]) : 0;
                            account.ChannelName = row["ChannelName"].ToString()?? "";
                            account.DashBoard = Convert.ToBoolean(row["Dashboard"]);
                            list.Add(account);
                        }
                    }
                }

            }
            return list;
        }
        #endregion
        #region Get Charts Filters Types
        public List<ChartFilters> GetChartFiltersList()
        {
            List<SqlParameter> param = new List<SqlParameter>() { };
            string queryName = "MOB_DASH_GetChartsFilterTypes";
            List<ChartFilters> respose = FetchChartFiltersList(queryName,param);
            return respose;
        }
        private List<ChartFilters> FetchChartFiltersList(string queryName, List<SqlParameter> param)
        {
            List<ChartFilters> list = new List<ChartFilters>();
            DataSet resultSet = null;
            if (!String.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, param, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable Table = resultSet.Tables[0];
                    if (Table.Rows.Count > 0)
                    {
                        foreach (DataRow row in Table.Rows)
                        {
                            ChartFilters obj = new ChartFilters();
                            obj.ID = !string.IsNullOrEmpty(row["ID"].ToString()) ? Convert.ToInt32(row["ID"]) : 0;
                            obj.Name = row["Name"].ToString()?? "";
                            list.Add(obj);
                        }
                    }
                }

            }
            return list;
        }
        #endregion
        #region Get Chart Statics
        public List<ChartStaticsData> GetChartStaticsData(DashBoardRequestDTO req)
        {
            List<SqlParameter> param = new List<SqlParameter>()
             {
                new  SqlParameter("@ChannelID",req.ChannelId),
                new  SqlParameter("@UserID",req.UserMKTID),
                new  SqlParameter("@FromDate",req.FromDate),
                new  SqlParameter("@ToDate",req.ToDate),
                new  SqlParameter("@ChartTypeID",req.ChartType),
             };
            string queryName = "MOB_DASH_GetChartsStats";
            List<ChartStaticsData> response = FetchChartStaticsData(queryName, param);
            return response;
        }
        private List<ChartStaticsData> FetchChartStaticsData(string queryName, List<SqlParameter> param)
        {
            List<ChartStaticsData> list = new List<ChartStaticsData>();
            DataSet resultSet = null;
            if (!String.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, param, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable Table = resultSet.Tables[0];
                    if (Table.Rows.Count > 0)
                    {
                        foreach (DataRow row in Table.Rows)
                        {
                            ChartStaticsData obj = new ChartStaticsData();
                            obj.Count = !string.IsNullOrEmpty(row["Count"].ToString()) ? Convert.ToInt32(row["Count"]) : 0;
                            obj.Date =!string.IsNullOrEmpty(row["Date"].ToString()) ? Convert.ToDateTime(row["Date"].ToString()).ToString("dd/MM") :string.Empty;
                            list.Add(obj);
                        }
                    }
                }

            }
            return list;
        }
        #endregion
    }
}
