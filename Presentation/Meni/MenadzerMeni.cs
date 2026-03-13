using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Servisi;

namespace Presentation.Meni
{
    public class MenadzerMeni
    {
        private readonly IPrijemServis prijemServis;
        private readonly INaplataServis naplataServis;
        private readonly Korisnik korisnik;

        public MenadzerMeni(IPrijemServis prijem, INaplataServis naplata, Korisnik kor)
        {
            prijemServis = prijem;
            naplataServis = naplata;
            korisnik = kor;
        }

        public void PrikaziMeni()
        {
            Console.WriteLine("\n============================================ MENI MENADŽERA ===========================================");

            bool kraj = false;
            while (!kraj)
            {
                Console.WriteLine("\n1. Dodaj vozilo na servis");
                Console.WriteLine("2. Pregled svih vozila");
                Console.WriteLine("3. Pregled svih izdatih računa");
                Console.WriteLine("4. Odjavite se");
                Console.Write("Opcija: ");
                string? opcija = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(opcija))
                    continue;

                switch (opcija.Trim())
                {
                    case "1":
                        DodajVozilo();
                        break;
                    case "2":
                        PregledVozila();
                        break;
                    case "3":
                        PregledRacuna();
                        break;
                    case "4":
                        kraj = true;
                        break;
                    default:
                        Console.WriteLine("Nepoznata opcija. Pokušajte ponovo.");
                        break;
                }
            }
        }

        private void DodajVozilo()
        {
            Console.WriteLine("\n--- Prijem vozila na servis ---");

            Console.Write("Registarski broj: ");
            string regBroj = Console.ReadLine() ?? "";

            Console.Write("Marka: ");
            string marka = Console.ReadLine() ?? "";

            Console.Write("Model: ");
            string model = Console.ReadLine() ?? "";

            Console.WriteLine("Tip vozila: 1 - Putničko, 2 - Teretno, 3 - Motocikl");
            Console.Write("Tip: ");
            string tipUnos = Console.ReadLine() ?? "1";
            TipVozila tip = tipUnos switch
            {
                "2" => TipVozila.TERETNO,
                "3" => TipVozila.MOTOCIKL,
                _ => TipVozila.PUTNICKO
            };

            Console.Write("Procenjena cena usluge (RSD): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal cena))
                cena = 5000;

            Vozilo vozilo = prijemServis.DodajVoziloNaServis(regBroj.Trim(), marka.Trim(), model.Trim(), tip, cena);

            if (vozilo.Id != 0)
                Console.WriteLine($"\nVozilo '{marka} {model}' (reg. br: {regBroj}) uspešno primljeno na servis!");
            else
                Console.WriteLine("\nNije moguće dodati vozilo. Proverite da li je servis pun ili vozilo već postoji.");
        }

        private void PregledVozila()
        {
            Console.WriteLine("\n============================================ SVA VOZILA =============================================");
            var vozila = prijemServis.PregledSvihVozila();
            Console.WriteLine(Vozilo.Header());
            foreach (var v in vozila)
                Console.WriteLine(v);
            Console.WriteLine(new string('=', 80));
        }

        private void PregledRacuna()
        {
            Console.WriteLine("\n============================================ SVI RAČUNI =============================================");
            var racuni = naplataServis.PregledSvihRacuna();
            Console.WriteLine(RacunZaUslugu.Header());
            foreach (var r in racuni)
                Console.WriteLine(r);
            Console.WriteLine(new string('=', 75));
        }
    }
}