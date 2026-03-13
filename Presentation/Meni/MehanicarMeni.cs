using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Modeli;
using Domain.Servisi;

namespace Presentation.Meni
{
    public class MehaničarMeni
    {
        private readonly INaplataServis naplataServis;
        private readonly Korisnik korisnik;

        public MehaničarMeni(INaplataServis naplata, Korisnik kor)
        {
            naplataServis = naplata;
            korisnik = kor;
        }

        public void PrikaziMeni()
        {
            Console.WriteLine("\n============================================ MENI MEHANIČARA =========================================");

            bool kraj = false;
            while (!kraj)
            {
                Console.WriteLine("\n1. Pregled vozila koja čekaju servis");
                Console.WriteLine("2. Završi servis i izdaj račun");
                Console.WriteLine("3. Odjavite se");
                Console.Write("Opcija: ");
                string? opcija = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(opcija))
                    continue;

                switch (opcija.Trim())
                {
                    case "1":
                        PregledVozilaZaServis();
                        break;
                    case "2":
                        ZavrsiServis();
                        break;
                    case "3":
                        kraj = true;
                        break;
                    default:
                        Console.WriteLine("Nepoznata opcija. Pokušajte ponovo.");
                        break;
                }
            }
        }

        private void PregledVozilaZaServis()
        {
            Console.WriteLine("\n====================================== VOZILA KOJA ČEKAJU SERVIS =====================================");
            var vozila = naplataServis.PregledVozilaKojaCekajuServis().ToList();

            if (!vozila.Any())
            {
                Console.WriteLine("Nema vozila koja čekaju servis.");
                return;
            }

            Console.WriteLine(Vozilo.Header());
            foreach (var v in vozila)
                Console.WriteLine(v);
            Console.WriteLine(new string('=', 80));
        }

        private void ZavrsiServis()
        {
            Console.WriteLine("\n--- Završetak servisa ---");

            var vozila = naplataServis.PregledVozilaKojaCekajuServis().ToList();

            if (!vozila.Any())
            {
                Console.WriteLine("Nema vozila koja čekaju servis.");
                return;
            }

            Console.WriteLine("Vozila koja čekaju servis:");
            for (int i = 0; i < vozila.Count; i++)
                Console.WriteLine($"{i + 1}. {vozila[i].Marka} {vozila[i].Model} (reg: {vozila[i].RegistarskiBroj}, ID: {vozila[i].Id})");

            Console.Write("\nUnesite ID vozila za koje završavate servis: ");
            if (!long.TryParse(Console.ReadLine(), out long voziloId))
            {
                Console.WriteLine("Nevažeći ID vozila.");
                return;
            }

            RacunZaUslugu racun = naplataServis.ZavrsiServisIIzdajRacun(voziloId, korisnik.ImePrezime);

            if (racun.Id != 0)
            {
                Console.WriteLine($"\nServis uspešno završen!");
                Console.WriteLine($"Mehaničar: {racun.ImeMehanicara}");
                Console.WriteLine($"Datum i vreme: {racun.DatumVremeIzdavanja:dd.MM.yyyy HH:mm}");
                Console.WriteLine($"Ukupan iznos: {racun.UkupanIznos:F2} RSD");
            }
            else
            {
                Console.WriteLine("\nNeuspešno završavanje servisa. Proverite ID vozila.");
            }
        }
    }
}