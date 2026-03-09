using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Modeli;

namespace Domain.Repozitorijumi
{
    public interface IRacuniRepozitorijum
    {
        public RacunZaUslugu DodajRacun(RacunZaUslugu racun);
        public RacunZaUslugu PronadjiRacunPoId(long id);
        public IEnumerable<RacunZaUslugu> SviRacuni();
    }
}
