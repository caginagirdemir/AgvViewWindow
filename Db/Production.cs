using System;
using System.Collections.Generic;

namespace AgvViewWindow.Db;

public partial class Production
{
    public int TableId { get; set; }

    public int? ProcId { get; set; }

    public int? MachineId { get; set; }

    public DateTime? BeginTime { get; set; }

    public int? PickUpSymbol { get; set; }

    public int? TargetSymbol { get; set; }

    public int? ItemType { get; set; }

    public int? IntStatus { get; set; }

    public DateTime? EndTime { get; set; }

    public int? ExecDur { get; set; }

    public double? TotalDist { get; set; }
}
