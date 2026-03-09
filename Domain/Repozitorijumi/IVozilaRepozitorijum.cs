using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Modeli;

namespace Domain.Repozitorijumi
{
    public interface IVozilaRepozitorijum
    {
        public Vozilo DodajVozilo(Vozilo vozilo);
        public Vozilo PronadjiVoziloPoId(long id);
        public Vozilo PronadjiVoziloPoRegistarskombBroju(string registarskiBroj);
        public bool AzurirajVozilo(Vozilo vozilo);
        public IEnumerable<Vozilo> SvaVozila();
        public IEnumerable<Vozilo> VozilaKojaCekajuServis();
        public int BrojVozilaKojaCekaju();
    }
}