using BokhandelApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[0] Gå tillbaka till huvudmenyn");
        Console.ResetColor();

        Console.WriteLine("\nDitt val: (ange siffra) ");
        string input = Console.ReadLine();

        switch (input)
        {
            case "0":
                return;
            case "1":
                VisaLagersaldo(butik);
                break;
            case "2":
                AndraLagersaldo(butik);
                break;
            case "4":
                TaBortBok(butik);
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ogiltigt val.");
                Console.ResetColor();
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

    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("\nTryck Enter för att gå tillbaka.");
    Console.ResetColor();
    Console.ReadLine();
}

void AndraLagersaldo(Butiker butik)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"                                        --- ÄNDRA LAGERSALDO: {butik.Butiksnamn} ---");
    Console.ResetColor();

    var saldoButik = db.LagerSaldos
        .Include(l => l.IsbnNavigation)
        .Where(l => l.ButikId == butik.Id)
        .ToList();

    if (saldoButik.Count == 0)
    {
        Console.WriteLine("Butiken har inga böcker/saknar saldo");
        Console.ReadKey();
        return;
    }

    Console.WriteLine("\nVälj vilken bok du vill ändra saldo på:");
    Console.WriteLine("-------------------------------------------");

    for (int i = 0; i < saldoButik.Count; i++)
    {
        var titelPlats = saldoButik[i];
        Console.WriteLine($"[{i + 1}] {titelPlats.IsbnNavigation.Titel} (Saldo: {titelPlats.Antal})");
    }

    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("[0] Gå tillbaka till huvudmenyn");
    Console.ResetColor();

    Console.Write("\nAnge vilken bok du vill ändra lagersaldo på: ");
    string saldoInput = Console.ReadLine();

    if (int.TryParse(saldoInput, out int index))
    {
        if (index == 0) return;

        int saldoIndex = index - 1;

        if (saldoIndex >= 0 && saldoIndex < saldoButik.Count)
        {
            var titelPlats = saldoButik[saldoIndex];

            Console.WriteLine($"\n Vald bok: {titelPlats.IsbnNavigation.Titel}");
            Console.WriteLine($"Nuvarande antal: {titelPlats.Antal}");

            Console.Write("Ange nytt lagersaldo: ");
            if (int.TryParse(Console.ReadLine(), out int nyttSaldo))
            {
                titelPlats.Antal = nyttSaldo;

                db.SaveChanges();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Saldot är uppdaterat!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor= ConsoleColor.Red;
                Console.WriteLine("Ogiltigt val. Du måste skriva en siffra!");
                Console.ResetColor();
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ogiltigt val. Siffran finns inte i listan!");
            Console.ResetColor();
        }
    }

    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Ogiltig inmatning: Du måste skriva en siffra.");
        Console.ResetColor();
    }

    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("\nTryck Enter för att gå tillbaka.");
    Console.ResetColor();
    Console.ReadLine();
}

void TaBortBok(Butiker butik)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"                                        --- TA BORT BOK UR SORTIMENTET: {butik.Butiksnamn} ---");
    Console.ResetColor();

    var saldoButik = db.LagerSaldos
        .Include(l => l.IsbnNavigation)
        .Where(l => l.ButikId == butik.Id)
        .ToList();

    if (saldoButik.Count == 0)
    {
        Console.WriteLine("Butiken har inga böcker att ta bort.");
        Console.ReadKey();
        return;
    }

    Console.WriteLine("\nVälj vilken bok som ska tas bort ur sortimentet:");
    Console.WriteLine("------------------------------------------------------");

    for (int i = 0; i < saldoButik.Count; i++)
    {
        Console.WriteLine($"[{i + 1}] {saldoButik[i].IsbnNavigation.Titel}");
    }

    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("[0] Avbryt");
    Console.ResetColor();

    Console.Write("\nAnge siffra: ");
    string input = Console.ReadLine();

    if (int.TryParse(input, out int index))
    {
        if (index == 0) return;

        int saldoIndex = index - 1;

        if (saldoIndex >= 0 && saldoIndex < saldoButik.Count)
        {
            var taBort = saldoButik[saldoIndex];

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nÄr du säker på att du vill ta bort '{taBort.IsbnNavigation.Titel}'?");
            Console.ResetColor();
            Console.WriteLine("\nVIKTIGT: Bekräfta genom att skriva [JA], avbryt med [Enter].");

            string svarInput = Console.ReadLine().ToLower();

            if (svarInput == "ja")
            {
                db.LagerSaldos.Remove(taBort);
                db.SaveChanges();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nBorttagning har lyckats. Boken är nu borttagen ur sortimentet.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor= ConsoleColor.Red;
                Console.WriteLine("\nBorttagning misslyckats. Avbryter åtgärden.");
                Console.ResetColor();
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nOgiltigt val. Siffran finns inte i listan!");
            Console.ResetColor();
        }
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Ogiltig inmatning: Du måste skriva en siffra.");
        Console.ResetColor();
    }

    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("\nTryck Enter för att gå tillbaka.");
    Console.ResetColor();
    Console.ReadLine();
}

