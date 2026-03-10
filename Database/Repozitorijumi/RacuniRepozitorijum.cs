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
    public class RacuniRepozitorijum : IRacuniRepozitorijum
    {
        IBazaPodataka bazaPodataka;

        public RacuniRepozitorijum(IBazaPodataka baza)
        {
            bazaPodataka = baza;
        }

        public RacunZaUslugu DodajRacun(RacunZaUslugu racun)
        {
            try
            {
                racun.Id = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                bazaPodataka.Tabele.Racuni.Add(racun);
                bool uspesno = bazaPodataka.SacuvajPromene();

                if (uspesno)
                    return racun;
                else
                    return new RacunZaUslugu();
            }
            catch
            {
                return new RacunZaUslugu();
            }
        }

        public RacunZaUslugu PronadjiRacunPoId(long id)
        {
            try
            {
                foreach (var racun in bazaPodataka.Tabele.Racuni)
                {
                    if (racun.Id == id)
                        return racun;
                }
                return new RacunZaUslugu();
            }
            catch
            {
                return new RacunZaUslugu();
            }
        }

        public IEnumerable<RacunZaUslugu> SviRacuni()
        {
            try
            {
                return bazaPodataka.Tabele.Racuni;
            }
            catch
            {
                return [];
            }
        }
    }
}