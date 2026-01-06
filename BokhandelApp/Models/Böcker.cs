using System;
using System.Collections.Generic;

namespace BokhandelApp.Models;

public partial class Böcker
{
    public string Isbn13 { get; set; } = null!;

    public string? Titel { get; set; }

    public string? Språk { get; set; }

    public decimal? Pris { get; set; }

    public DateOnly? Utgivningsdatum { get; set; }

    public int? FörfattareId { get; set; }

    public int? FörlagId { get; set; }

    public int? GenreId { get; set; }

    public virtual Författare? Författare { get; set; }

    public virtual Förlag? Förlag { get; set; }

    public virtual Genre? Genre { get; set; }

    public virtual ICollection<LagerSaldo> LagerSaldos { get; set; } = new List<LagerSaldo>();
}
