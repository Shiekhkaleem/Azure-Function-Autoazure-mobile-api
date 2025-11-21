using AutoAzureMob.Models.Models.DashBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.VM.DashBoard
{
    public class DashBoardVM2
    {
        public DashboardData DashboardData { get; set; } = new DashboardData();
        public List<ChartStaticsData> ChartData { get; set; } = new List<ChartStaticsData>();
        public string TotalNotification { get; set; }
    }
}
