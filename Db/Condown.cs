using System;
using System.Collections.Generic;

namespace AgvViewWindow.Db;

public partial class Condown
{
    public int CondownId { get; set; }

    public DateTime CondownDownTime { get; set; }

    public DateTime CondownUpTime { get; set; }
}
