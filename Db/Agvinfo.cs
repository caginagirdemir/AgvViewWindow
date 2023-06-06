using System;
using System.Collections.Generic;

namespace AgvViewWindow.Db;

public partial class Agvinfo
{
    public int AgvId { get; set; }

    public string AgvName { get; set; } = null!;

    public string AgvGroupName { get; set; } = null!;
}
