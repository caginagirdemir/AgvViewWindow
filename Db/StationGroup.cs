using System;
using System.Collections.Generic;

namespace AgvViewWindow.Db;

public partial class StationGroup
{
    public int StationGroupId { get; set; }

    public string StationGroupName { get; set; } = null!;

    public int SupermarketId { get; set; }
}
