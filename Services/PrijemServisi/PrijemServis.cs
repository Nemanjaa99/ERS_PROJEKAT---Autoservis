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

namespace Services.PrijemServisi
{
    public class PrijemServis : IPrijemServis
    {
        IVozilaRepozitorijum vozilaRepozitorijum;
        ILoggerServis loggerServis;

        public PrijemServis(IVozilaRepozitorijum vozilaRepo, ILoggerServis logger)
        {
            vozilaRepozitorijum = vozilaRepo;
            loggerServis = logger;
        }

        public Vozilo DodajVoziloNaServis(string registarskiBroj, string marka, string model, TipVozila tip, decimal procenjenaCena)
        {
            try
            {
                // Proveriti da li vozilo već postoji
                Vozilo postoji = vozilaRepozitorijum.PronadjiVoziloPoRegistarskombBroju(registarskiBroj);
                if (postoji.Id != 0)
                {
                    loggerServis.EvidentirajDogadjaj(TipEvidencije.WARNING, $"Vozilo sa registarskim brojem '{registarskiBroj}' već postoji u sistemu.");
                    return new Vozilo();
                }

                // Proveriti kapacitet
                int trenutniBroj = vozilaRepozitorijum.BrojVozilaKojaCekaju();
                if (trenutniBroj >= AutoservisKonstante.MAX_VOZILA_NA_SERVISU)
                {
                    loggerServis.EvidentirajDogadjaj(TipEvidencije.WARNING, $"Autoservis je pun! Maksimalan broj vozila ({AutoservisKonstante.MAX_VOZILA_NA_SERVISU}) je dostignut.");
                    return new Vozilo();
                }

                // Dodati vozilo
                Vozilo novoVozilo = new Vozilo(registarskiBroj, marka, model, tip, procenjenaCena);
                Vozilo dodato = vozilaRepozitorijum.DodajVozilo(novoVozilo);

                if (dodato.Id != 0)
                {
                    loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, $"Vozilo '{marka} {model}' (reg. br: {registarskiBroj}) uspešno primljeno na servis.");
                    return dodato;
                }
                else
                {
                    loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, $"Neuspešan prijem vozila '{registarskiBroj}' na servis.");
                    return new Vozilo();
                }
            }
            catch
            {
                loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Neuspešan prijem vozila na servis.");
                return new Vozilo();
            }
        }

        public IEnumerable<Vozilo> PregledSvihVozila()
        {
            try
            {
                loggerServis.EvidentirajDogadjaj(TipEvidencije.INFO, "Pregled svih vozila na servisu.");
                return vozilaRepozitorijum.SvaVozila();
            }
            catch
            {
                loggerServis.EvidentirajDogadjaj(TipEvidencije.ERROR, "Neuspešan pregled vozila.");
                return [];
            }
        }
    }
}