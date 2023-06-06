using AgvViewWindow.Db;
using Microsoft.AspNetCore.Mvc;

namespace AgvViewWindow.Models
{
    public class ReportModel
    {
        public List<string> dates { get; set; } = new List<string>();

        public double daily_average_min { get; set; }
        public double daily_average_speed { get; set; }
        public double optimum_speed { get; set; }

        public List<string> agvTypeName { get; set; } = new List<string>();
        public List<string> agvTypeSpeedDaily { get; set; } = new List<string>();
        public string agvTypeSpeedDailyCount { get; set; } = new string("0");
        public List<string> agvTypeSpeedMonthly { get; set; } = new List<string>();
        public string agvTypeSpeedMonthlyCount { get; set; } = new string("0");
        public List<string> agvTypeSpeedYearly { get; set; } = new List<string>();
        public string agvTypeSpeedYearlyCount { get; set; } = new string("0");

        public List<string> stationGroupTypeName { get; set; } = new List<string>();
        public List<string> stationTypeName { get; set; } = new List<string>();
        public List<string> stationTypeSpeedDaily { get; set; } = new List<string>();
        public List<string> stationTypeSpeedMonthly { get; set; } = new List<string>();
        public List<string> stationTypeSpeedYearly { get; set; } = new List<string>();

        public List<Delivery> deliveries { get; set; } = new List<Delivery>();
      
        

    }
}
