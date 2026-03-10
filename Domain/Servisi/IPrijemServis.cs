using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Enumeracije;
using Domain.Modeli;

namespace Domain.Servisi
{
    public interface IPrijemServis
    {
        public Vozilo DodajVoziloNaServis(string registarskiBroj, string marka, string model, TipVozila tip, decimal procenjenaCena);
        public IEnumerable<Vozilo> PregledSvihVozila();
    }
}