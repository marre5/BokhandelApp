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

        Console.WriteLine("\nDitt val (ange ID): ");
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

        Console.WriteLine("\nDitt val: ");
        string input = Console.ReadLine();

        if (input == "0") return;
    }
}

