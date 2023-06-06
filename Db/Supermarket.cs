using System;
using System.Collections.Generic;

namespace AgvViewWindow.Db;

public partial class Supermarket
{
    public int SupermarketId { get; set; }

    public string SupermarketName { get; set; } = null!;

    public int SupermarketSymbolicPoint { get; set; }

    public int SupermarketSymbolicPrePoint { get; set; }
}
