using System;
using System.Collections.Generic;

namespace BokhandelApp.Models;

public partial class Förlag
{
    public int Id { get; set; }

    public string? Namn { get; set; }

    public string? Kontaktnummer { get; set; }

    public virtual ICollection<Böcker> Böckers { get; set; } = new List<Böcker>();
}
