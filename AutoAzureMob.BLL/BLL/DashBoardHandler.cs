using AutoAzureMob.DAL.DAL;
using AutoAzureMob.Models.DTO.DashBoardDTO;
using AutoAzureMob.Models.DTO.UserDTO;
using AutoAzureMob.Models.Models;
using AutoAzureMob.Models.Models.DashBoard;
using AutoAzureMob.Models.Models.Response;
using AutoAzureMob.Models.VM.DashBoard;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.BLL.BLL
{
    public class DashBoardHandler  :BaseHandler
    {
        private readonly DashBoardDAO dashBoardDAO;
        private readonly NotificationHandler notificationHandler;
        private readonly IConfiguration config;
        public DashBoardHandler(ExecuteContext executeContext, IConfiguration _config) : base(executeContext, _config)
        {
            config = _config;
            dashBoardDAO = new DashBoardDAO(executeContext, config);
            notificationHandler = new NotificationHandler(executeContext, config);
        }
        public ResponseModel<DasBoardVM> GetDashBoardData(DashBoardRequestDTO req)
        {
            ResponseModel<DasBoardVM> response = new ResponseModel<DasBoardVM>();
            response.Content = new DasBoardVM();
            response.Content.DashboardData = dashBoardDAO.GetDashboardData(req);
            response.Content.ChartData = dashBoardDAO.GetChartStaticsData(req);
            response.Description = "Dash board data not found.";
            if (response.Content !=null)
            {
                response.Success = true;
                response.Description = "Dash board data";
            }
            return response;
        }
        public ResponseModel<DashBoardVM2> GetDashBoardDataV2(DashBoardRequestDTO2 req)
        {
            ResponseModel<DashBoardVM2> response = new ResponseModel<DashBoardVM2>();
            response.Content = new DashBoardVM2();
            DashBoardRequestDTO req2 = new()
            {
                ChannelId = req.ChannelId,
                UserMKTID = req.UserMKTID,
                FromDate = req.FromDate,
                ToDate = req.ToDate,
                ChartType = req.ChartType
            };
            response.Content.DashboardData = dashBoardDAO.GetDashboardData(req2);
            response.Content.ChartData = dashBoardDAO.GetChartStaticsData(req2);
            var totalResponse = notificationHandler.GetNotificationsTotal(req.CompanyId);
            response.Content.TotalNotification = totalResponse.Content; 
            response.Description = "Dash board data not found.";
            if (response.Content != null)
            {
                response.Success = true;
                response.Description = "Dash board data";
            }
            return response;
        }
        public ResponseModel<List<LinkedAccounts>> GetLinkedAccountsList(AccountsDTO req)
        {
            ResponseModel<List<LinkedAccounts>> response = new ResponseModel<List<LinkedAccounts>>();
            string json = File.ReadAllText("Logos.json");
            List<Logos> logos = new List<Logos>();
            logos = JsonConvert.DeserializeObject<List<Logos>>(json);
            response.Content = dashBoardDAO.GetLinkedAccounts(req);
            response.Description = "Accounts not found.";
            response.Success = true;
            if (response.Content != null && response.Content.Count > 0)
            {
                response.Content.ForEach(e => { e.ChannelThumbnail = logos.FirstOrDefault(y => y.Id == e.ChannelId).Url; });
                response.Description = "List of linked accounts.";
            }
            return response;
        }
        public ResponseModel<List<ChartFilters>> GetChartFiltersList()
        {
            ResponseModel<List<ChartFilters>> response = new ResponseModel<List<ChartFilters>>();
            response.Content = dashBoardDAO.GetChartFiltersList();
            response.Description = "Chart filter types not found.";
            response.Success = true;
            if (response.Content != null && response.Content.Count > 0)
            {
                response.Description = "List of chart filter types";
            }
            return response;
        }
        public ResponseModel<List<ChartStaticsData>> GetChartStaticsData(DashBoardRequestDTO req)
        {
            ResponseModel<List<ChartStaticsData>> response = new ResponseModel<List<ChartStaticsData>>();
            response.Content = dashBoardDAO.GetChartStaticsData(req);
            response.Description = "Chart statics data not found.";
            response.Success = true;
            if (response.Content != null && response.Content.Count > 0)
            {
                response.Description = "Chart statics data";
            }
            return response;
        }
        public ResponseModel<List<Logos>> GetMarketLogos()
        {
            ResponseModel<List<Logos>> response = new ResponseModel<List<Logos>>();
             List<Logos> logos = new List<Logos>(); 
            string json = File.ReadAllText("Logos.json");
            logos = JsonConvert.DeserializeObject<List<Logos>>(json);
            response.Content = logos;
            response.Description = "Market Logos";
            response.Success = true;
            return response;
        }
    }
}
