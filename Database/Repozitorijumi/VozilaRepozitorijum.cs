using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.BazaPodataka;
using Domain.Modeli;
using Domain.Repozitorijumi;

namespace Database.Repozitorijumi
{
    public class VozilaRepozitorijum : IVozilaRepozitorijum
    {
        IBazaPodataka bazaPodataka;

        public VozilaRepozitorijum(IBazaPodataka baza)
        {
            bazaPodataka = baza;
        }

        public Vozilo DodajVozilo(Vozilo vozilo)
        {
            try
            {
                vozilo.Id = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                bazaPodataka.Tabele.Vozila.Add(vozilo);
                bool uspesno = bazaPodataka.SacuvajPromene();

                if (uspesno)
                    return vozilo;
                else
                    return new Vozilo();
            }
            catch
            {
                return new Vozilo();
            }
        }

        public Vozilo PronadjiVoziloPoId(long id)
        {
            try
            {
                foreach (var vozilo in bazaPodataka.Tabele.Vozila)
                {
                    if (vozilo.Id == id)
                        return vozilo;
                }
                return new Vozilo();
            }
            catch
            {
                return new Vozilo();
            }
        }

        public Vozilo PronadjiVoziloPoRegistarskombBroju(string registarskiBroj)
        {
            try
            {
                foreach (var vozilo in bazaPodataka.Tabele.Vozila)
                {
                    if (vozilo.RegistarskiBroj == registarskiBroj)
                        return vozilo;
                }
                return new Vozilo();
            }
            catch
            {
                return new Vozilo();
            }
        }

        public bool AzurirajVozilo(Vozilo vozilo)
        {
            try
            {
                for (int i = 0; i < bazaPodataka.Tabele.Vozila.Count; i++)
                {
                    if (bazaPodataka.Tabele.Vozila[i].Id == vozilo.Id)
                    {
                        bazaPodataka.Tabele.Vozila[i] = vozilo;
                        return bazaPodataka.SacuvajPromene();
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Vozilo> SvaVozila()
        {
            try
            {
                return bazaPodataka.Tabele.Vozila;
            }
            catch
            {
                return [];
            }
        }

        public IEnumerable<Vozilo> VozilaKojaCekajuServis()
        {
            try
            {
                List<Vozilo> rezultat = [];
                foreach (var vozilo in bazaPodataka.Tabele.Vozila)
                {
                    if (!vozilo.Servisirano)
                        rezultat.Add(vozilo);
                }
                return rezultat;
            }
            catch
            {
                return [];
            }
        }

        public int BrojVozilaKojaCekaju()
        {
            try
            {
                int broj = 0;
                foreach (var vozilo in bazaPodataka.Tabele.Vozila)
                {
                    if (!vozilo.Servisirano)
                        broj++;
                }
                return broj;
            }
            catch
            {
                return 0;
            }
        }
    }
}