using System;
using System.Collections.Generic;

namespace BokhandelApp.Models;

public partial class TitlarPerFörfattare
{
    public string? Namn { get; set; }

    public string? Ålder { get; set; }

    public int? Titlar { get; set; }

    public decimal? Lagervärde { get; set; }
}
