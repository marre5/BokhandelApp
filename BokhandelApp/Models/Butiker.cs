using System;
using System.Collections.Generic;

namespace BokhandelApp.Models;

public partial class Butiker
{
    public int Id { get; set; }

    public string? Butiksnamn { get; set; }

    public string? Gatuadress { get; set; }

    public string? Postnummer { get; set; }

    public string? Stad { get; set; }

    public virtual ICollection<LagerSaldo> LagerSaldos { get; set; } = new List<LagerSaldo>();
}
