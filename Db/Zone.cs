using System;
using System.Collections.Generic;

namespace AgvViewWindow.Db;

public partial class Zone
{
    public int ZoneId { get; set; }

    public string ZoneName { get; set; } = null!;

    public double ZoneXZero { get; set; }

    public double ZoneYZero { get; set; }

    public double ZoneXOne { get; set; }

    public double ZoneYOne { get; set; }
}
