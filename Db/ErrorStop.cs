using System;
using System.Collections.Generic;

namespace AgvViewWindow.Db;

public partial class Errorstop
{
    public int ErrorStopId { get; set; }

    public int MachineId { get; set; }

    public string ErrorId { get; set; } = null!;

    public double PosX { get; set; }

    public double PosY { get; set; }

    public int NavitrolState { get; set; }

    public DateTime TrackBeginTime { get; set; }

    public string StopDuration { get; set; } = null!;
}
