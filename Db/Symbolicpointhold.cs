using System;
using System.Collections.Generic;

namespace AgvViewWindow.Db;

public partial class Symbolicpointhold
{
    public int SymbolicPointId { get; set; }

    public int MachineTypeId { get; set; }

    public int DefaultReleaseLocation { get; set; }

    public int AtOneLocation { get; set; }

    public int AtTwoLocation { get; set; }

    public int AtThreeLocation { get; set; }

    public long? ReleaseTimeoutSecond { get; set; }
}
