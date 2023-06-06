using System;
using System.Collections.Generic;

namespace AgvViewWindow.Db;

public partial class Target
{
    public int BatteryLevel { get; set; }

    public double AgvOptimumSpeed { get; set; }

    public int AgvUtilRatio { get; set; }

    public int SmLoad { get; set; }

    public int SmWaiting { get; set; }

    public int WsWaiting { get; set; }
}
