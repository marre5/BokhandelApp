using System;
using System.Collections.Generic;

namespace BokhandelApp.Models;

public partial class Genre
{
    public int Id { get; set; }

    public string Namn { get; set; } = null!;

    public virtual ICollection<Böcker> Böckers { get; set; } = new List<Böcker>();
}
