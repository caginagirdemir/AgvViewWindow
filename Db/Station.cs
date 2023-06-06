using System;
using System.Collections.Generic;

namespace AgvViewWindow.Db;

public partial class Station
{
    public int StationId { get; set; }

    public string StationName { get; set; } = null!;

    public int StationSymbolicPointId { get; set; }

    public int StationGroupId { get; set; }

    public string StationGroupName { get; set; } = null!;

    public int StationSymbolicPrePoint { get; set; }
}
