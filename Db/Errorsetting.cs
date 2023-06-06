using System;
using System.Collections.Generic;

namespace AgvViewWindow.Db;

public partial class Errorsetting
{
    public int ErrorId { get; set; }

    public string ErrorName { get; set; } = null!;

    public string ErrorGroup { get; set; } = null!;

    public bool DashboardVisible { get; set; }

    public bool RecordFlag { get; set; }

    public bool? ShortList { get; set; }

    public int Duration { get; set; }
}
