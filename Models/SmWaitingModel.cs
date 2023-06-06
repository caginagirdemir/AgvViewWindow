namespace AgvViewWindow.Models
{
    public class SmWaitingModel
    {
        public List<string> dates { get; set; } = new List<string>();
        public List<string> SuperMarketNames { get; set; } = new List<string>();
        public List<double[]> LoadTimeLine { get; set; } = new List<double[]>();
        public List<double[]> LoadTimeLineCount { get; set; } = new List<double[]>();

        public List<string> LoadSMValueDaily { get; set; } = new List<string>();
        public List<string> LoadSMValueMonthly { get; set; } = new List<string>();
        public List<string> LoadSMValueYearly { get; set; } = new List<string>();

        public List<double[]> WaitingTimeLine { get; set; } = new List<double[]>();
        public List<double[]> WaitingTimeLineCount { get; set; } = new List<double[]>();

        public List<string> WaitingSMValueDaily { get; set; } = new List<string>();
        public List<string> WaitingSMValueMonthly { get; set; } = new List<string>();
        public List<string> WaitingSMValueYearly { get; set; } = new List<string>();



        public List<string> TimeLineLoading { get; set; } = new List<string>();
        public List<string> TimeLineWaiting { get; set; } = new List<string>();
    }
}
