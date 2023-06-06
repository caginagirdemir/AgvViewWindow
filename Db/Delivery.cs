using System;
using System.Collections.Generic;

namespace AgvViewWindow.Db;

public partial class Delivery
{
    public int DeliveryId { get; set; }

    public int? MachineId { get; set; }

    public int? SupermarketId { get; set; }

    public int? DeliveredTargetId { get; set; }

    public int? TakePointId { get; set; }

    public DateTime RequestTime { get; set; }

    public DateTime? ResponseTime { get; set; }

    public DateTime? PrereleasePointTime { get; set; }

    public DateTime? DropTime { get; set; }

    public DateTime? ToMarketTime { get; set; }

    public DateTime? AtSupermarketTime { get; set; }

    public DateTime? AtPoleTime { get; set; }

    public DateTime? LastAtSupermarketTime { get; set; }

    public DateTime? LastAtPoleTime { get; set; }

    public double? AvgCycleSpeed { get; set; }
}
