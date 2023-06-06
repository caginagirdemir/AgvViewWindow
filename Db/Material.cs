using System;
using System.Collections.Generic;

namespace AgvViewWindow.Db;

public partial class Material
{
    public int MaterialId { get; set; }

    public string MaterialName { get; set; } = null!;

    public int SupermarketId { get; set; }

    public string SupermarketName { get; set; } = null!;

    public int StationGroupId { get; set; }

    public string StationGroupName { get; set; } = null!;

    public int StationId { get; set; }

    public string StationName { get; set; } = null!;
}
