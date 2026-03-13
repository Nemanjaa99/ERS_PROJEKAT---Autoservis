using Database.BazaPodataka;
using Database.Repozitorijumi;
using Domain.BazaPodataka;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Presentation.Authentifikacija;
using Presentation.Meni;
using Services.AutenftikacioniServisi;
using Services.LoggerServisi;
using Services.NaplataServisi;
using Services.PrijemServisi;

namespace Loger_Bloger
{
    public class Program
    {
        public static void Main()
        {
            // Baza podataka
            IBazaPodataka bazaPodataka = new JsonBazaPodataka();

            // Repozitorijumi
            IKorisniciRepozitorijum korisniciRepozitorijum = new KorisniciRepozitorijum(bazaPodataka);
            IVozilaRepozitorijum vozilaRepozitorijum = new VozilaRepozitorijum(bazaPodataka);
            IRacuniRepozitorijum racuniRepozitorijum = new RacuniRepozitorijum(bazaPodataka);

            // Servisi
            ILoggerServis loggerServis = new FileLoggerServis();
            IAutentifikacijaServis autentifikacijaServis = new AutentifikacioniServis(korisniciRepozitorijum, loggerServis);
            IPrijemServis prijemServis = new PrijemServis(vozilaRepozitorijum, loggerServis);

            // Inicijalni podaci - korisnici
            if (korisniciRepozitorijum.SviKorisnici().Count() == 0)
            {
                korisniciRepozitorijum.DodajKorisnika(new Korisnik("pera", "123", "Petar Petrović", TipKorisnika.MENADZER));
                korisniciRepozitorijum.DodajKorisnika(new Korisnik("marko", "123", "Marko Marković", TipKorisnika.MEHANICAR));
                korisniciRepozitorijum.DodajKorisnika(new Korisnik("ana", "456", "Ana Anić", TipKorisnika.MEHANICAR));
            }

            // Inicijalni podaci - vozila
            if (vozilaRepozitorijum.SvaVozila().Count() == 0)
            {
                vozilaRepozitorijum.DodajVozilo(new Vozilo("NS-123-AB", "Volkswagen", "Golf 7", TipVozila.PUTNICKO, 8500));
                vozilaRepozitorijum.DodajVozilo(new Vozilo("BG-456-CD", "Ford", "Transit", TipVozila.TERETNO, 12000));
                vozilaRepozitorijum.DodajVozilo(new Vozilo("NI-789-EF", "Honda", "CBR 600", TipVozila.MOTOCIKL, 4500));
            }

            // Autentifikacija
            Console.WriteLine("========================================= AUTOSERVIS =========================================");
            Console.WriteLine("                    Dobrodošli u sistem za upravljanje autoservisom");
            Console.WriteLine("==============================================================================================\n");

            AutentifikacioniMeni am = new AutentifikacioniMeni(autentifikacijaServis);
            Korisnik prijavljen = new Korisnik();

            while (am.TryLogin(out prijavljen) == false)
            {
                Console.WriteLine("Pogrešno korisničko ime ili lozinka. Pokušajte ponovo.\n");
            }

            Console.Clear();
            Console.WriteLine($"Uspešno ste prijavljeni kao: {prijavljen.ImePrezime} ({prijavljen.Uloga})");

            // Odrediti smenu na osnovu trenutnog vremena
            int trenutniSat = DateTime.Now.Hour;
            INaplataServis naplataServis;

            if (trenutniSat >= 8 && trenutniSat < 12)
            {
                naplataServis = new PrepodneNaplataServis(vozilaRepozitorijum, racuniRepozitorijum, loggerServis);
                Console.WriteLine("Aktivna smena: PRVA SMENA (8:00 - 12:00) - Popust 15%");
            }
            else
            {
                naplataServis = new PoslepodneNaplataServis(vozilaRepozitorijum, racuniRepozitorijum, loggerServis);
                Console.WriteLine("Aktivna smena: DRUGA SMENA (12:00 - 16:00) - Porez 10%");
            }

            // Prikaz menija na osnovu uloge
            if (prijavljen.Uloga == TipKorisnika.MENADZER)
            {
                MenadzerMeni meni = new MenadzerMeni(prijemServis, naplataServis, prijavljen);
                meni.PrikaziMeni();
            }
            else
            {
                MehaničarMeni meni = new MehaničarMeni(naplataServis, prijavljen);
                meni.PrikaziMeni();
            }

            Console.WriteLine("\nOdjavljeni ste iz sistema. Doviđenja!");
        }
    }
}