using BokhandelApp.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

using var db = new BokhandelDbContext();


Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("                                           VÄLKOMMEN TILL ADMIN-PORTALEN");
Console.ResetColor();
Console.WriteLine();
Console.WriteLine();
Console.WriteLine("Tryck på valfri tangent för att starta portalen...");
Console.ReadKey();

ValjButik();

void ValjButik()
{
    while (true)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("                                        --- HUVUDMENYN (BOKHANDEL ADMIN) ---");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine("Välj butik:");
        Console.WriteLine("-----------------");

        var butiker = db.Butikers.ToList();

        foreach (var b in butiker)
        {
            Console.WriteLine($"[{b.Id}] {b.Butiksnamn}");
        }
        Console.WriteLine("[0] Avsluta");

        Console.WriteLine("\nDitt val (ange siffra): ");
        string input = Console.ReadLine();

        if (input == "0") return;

        if (int.TryParse(input, out int valtId))
        {
            var valdButik = db.Butikers.FirstOrDefault(b => b.Id == valtId);

            if (valdButik != null)
            {
                ButiksMeny(valdButik);
            }
            else
            {
                Console.WriteLine("Butiken hittades inte. Tryck Enter.");
                Console.ReadLine();
            }
        }
        else
        {
            Console.WriteLine("Ogiligt val. Tryck Enter.");
            Console.ReadLine();
        }
    }
}

void ButiksMeny(Butiker butik)
{
    while (true)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"                                        --- {butik.Butiksnamn} (ADMIN) ---");
        Console.ResetColor();

        Console.WriteLine("\n Välj åtgärd:");
        Console.WriteLine("[1] Visa lagersaldo");
        Console.WriteLine("[2] Ändra lagersaldo");
        Console.WriteLine("[3] Lägg till ny bok i sortimentet");
        Console.WriteLine("[4] Ta bort bok ur sortimentet");
        Console.WriteLine("[0] Gå tillbaka till huvudmenyn");

        Console.WriteLine("\nDitt val: (ange siffra) ");
        string input = Console.ReadLine();

        switch (input)
        {
            case "1":
                VisaLagersaldo(butik);
                break;
            case "0":
                return;
            default:
                break;
        }
    }
}

void VisaLagersaldo(Butiker butik)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"                                        --- LAGERSALDO: {butik.Butiksnamn} ---");
    Console.ResetColor();
    Console.WriteLine();

    var lagerLista = db.LagerSaldos
        .Include(l => l.IsbnNavigation)
        .Where(l => l.ButikId == butik.Id)
        .ToList();

    if (lagerLista.Count == 0)
    {
        Console.WriteLine("Lagret är tomt");
    }
    else
    {
        Console.WriteLine("{0,-35} {1,-15} {2,5}", "Titel", "ISBN", "Antal");
        Console.WriteLine("------------------------------------------------------------------");

        foreach (var rad in lagerLista)
        {
            string titel = rad.IsbnNavigation.Titel.Length > 30
                ? rad.IsbnNavigation.Titel.Substring(0, 27) + "..."
                : rad.IsbnNavigation.Titel;

            Console.WriteLine("{0,-35} {1,-15} {2,5}", titel, rad.IsbnNavigation.Isbn13, rad.Antal);
        }
    }

    Console.WriteLine("\nTryck Enter för att gå tillbaka:");
    Console.ReadLine();
}

