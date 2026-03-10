using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Modeli;

namespace Domain.Servisi
{
    public interface INaplataServis
    {
        public RacunZaUslugu ZavrsiServisIIzdajRacun(long voziloId, string imeMehanicara);
        public IEnumerable<RacunZaUslugu> PregledSvihRacuna();
        public IEnumerable<Vozilo> PregledVozilaKojaCekajuServis();
    }
}