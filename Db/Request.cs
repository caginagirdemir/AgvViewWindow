using System;
using System.Collections.Generic;

namespace AgvViewWindow.Db;

public partial class Request
{
    public int RequestId { get; set; }

    public string RequestedMaterialName { get; set; } = null!;

    public int RequestedMaterialId { get; set; }

    public int SupermarketId { get; set; }

    public string SupermarketName { get; set; } = null!;

    public int StationGroupId { get; set; }

    public string StationGroupName { get; set; } = null!;

    public int StationId { get; set; }

    public string StationName { get; set; } = null!;

    public DateTime RequestTime { get; set; }

    public bool RequestStatus { get; set; }

    public bool ManuelRequestStatus { get; set; }

    public DateTime ResponseTime { get; set; }
}
