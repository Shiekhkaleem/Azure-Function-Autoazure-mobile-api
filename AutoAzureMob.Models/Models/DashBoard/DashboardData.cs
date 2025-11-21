using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Models.Models.DashBoard
{
    public class DashboardData
    {
        public string Visits { get; set; } = "0";
        public string Orders { get; set; } = "0";
        public string Shipments { get; set; } = "0";
        public string DelayedShipments { get; set; } = "0";
        public string Claims { get; set; } = "0";
        public string Questions { get; set; } = "0";
        public string Payments { get; set; } = "0";
        public string AveragePayments { get; set; } = "0";
        public string Accounts { get; set; } = "0";

    }
}
