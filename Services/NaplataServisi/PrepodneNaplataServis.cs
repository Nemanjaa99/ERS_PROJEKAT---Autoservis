using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Enumeracije;
using Domain.Konstante;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.NaplataServisi
{
    // Prva smena (8:00 - 12:00): primenjuje popust od 15%
    public class PrepodneNaplataServis : INaplataServis
    {
        IVozilaRepozitorijum vozilaRepozitorijum;
        IRacuniRepozitorijum racuniRepozitorijum;
        ILoggerServis loggerServis;

        public PrepodneNaplataServis(IVozilaRepozitorijum vozilaRepo, IRacuniRepozitorijum racuniRepo, ILoggerServis logger)
        {
            vozilaRepozitorijum = vozilaRepo;
            racuniRepozitorijum = racuniRepo;
            loggerServis = logger;
        }

        public RacunZaUslugu ZavrsiServisIIzdajRacun(long voziloId, string imeMehanicara)
        {
            try
            {
                Vozilo vozilo = vozilaRepozitorijum.PronadjiVoziloPoId(voziloId);

                if (vozilo.Id == 0)
                {
                    loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, $"Vozilo sa ID {voziloId} nije pronađeno.");
                    return new RacunZaUslugu();
                }

                if (vozilo.Servisirano)
                {
                    loggerServis.EvidentirajDogadjaj(TipEvidencije.WARNING, $"Vozilo '{vozilo.RegistarskiBroj}' je već servisirano.");
                    return new RacunZaUslugu();
                }

                // Prva smena - popust 15%
                decimal originalnaСena = vozilo.ProcenjenaCenaUsluge;
                decimal popust = originalnaСena * (decimal)AutoservisKonstante.POPUST_PRVA_SMENA;
                decimal krajnjiIznos = originalnaСena - popust;

                // Ažurirati vozilo kao servisirano
                vozilo.Servisirano = true;
                vozilaRepozitorijum.AzurirajVozilo(vozilo);

                // Kreirati i sačuvati račun
                RacunZaUslugu racun = new RacunZaUslugu(imeMehanicara, DateTime.Now, krajnjiIznos, voziloId);
                RacunZaUslugu dodat = racuniRepozitorijum.DodajRacun(racun);

                if (dodat.Id != 0)
                {
                    loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO,
                        $"Servis završen za vozilo '{vozilo.RegistarskiBroj}'. Originalna cena: {originalnaСena:F2} RSD, Popust: {popust:F2} RSD (15%), Ukupno: {krajnjiIznos:F2} RSD.");
                    return dodat;
                }
                else
                {
                    loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Neuspešno izdavanje računa.");
                    return new RacunZaUslugu();
                }
            }
            catch
            {
                loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Greška pri završetku servisa i izdavanju računa.");
                return new RacunZaUslugu();
            }
        }

        public IEnumerable<RacunZaUslugu> PregledSvihRacuna()
        {
            try
            {
                loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, "Pregled svih izdatih računa.");
                return racuniRepozitorijum.SviRacuni();
            }
            catch
            {
                loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Neuspešan pregled računa.");
                return [];
            }
        }

        public IEnumerable<Vozilo> PregledVozilaKojaCekajuServis()
        {
            try
            {
                loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, "Pregled vozila koja čekaju servis.");
                return vozilaRepozitorijum.VozilaKojaCekajuServis();
            }
            catch
            {
                loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Neuspešan pregled vozila koja čekaju servis.");
                return [];
            }
        }
    }
}