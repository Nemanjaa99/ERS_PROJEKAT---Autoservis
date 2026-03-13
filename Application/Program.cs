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
            INaplataServis naplataServis = new PrepodneNaplataServis(vozilaRepozitorijum, racuniRepozitorijum, loggerServis);

            // Inicijalni podaci
            if (korisniciRepozitorijum.SviKorisnici().Count() == 0)
            {
                korisniciRepozitorijum.DodajKorisnika(new Korisnik("menadzer", "123", "Petar Petrović", TipKorisnika.MENADZER));
            }

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
            Console.WriteLine($"Uspešno ste prijavljeni kao: {prijavljen.ImePrezime} ({prijavljen.Uloga})\n");

            // Direktno menadžer meni
            MenadzerMeni meni = new MenadzerMeni(prijemServis, naplataServis, prijavljen);
            meni.PrikaziMeni();

            Console.WriteLine("\nOdjavljeni ste iz sistema. Doviđenja!");
        }
    }
}