namespace AgvViewWindow.Models
{
    public class WsModel
    {
        public string date { get; set; }
        public List<string> workStationList { get; set; } = new List<string>();
        public List<string> toDropTime { get; set; } = new List<string>();
        public List<string> toOutTime { get; set; } = new List<string>();
        public List<string> stopedTime { get; set; } = new List<string>();
        public List<string> zones { get; set; } = new List<string>();
        public List<string> errorList { get; set; } = new List<string>();
    }
}
