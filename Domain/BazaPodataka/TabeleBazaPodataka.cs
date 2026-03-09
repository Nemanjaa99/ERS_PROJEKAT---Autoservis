using Domain.Modeli;

namespace Domain.BazaPodataka
{
    public class TabeleBazaPodataka
    {
        public List<Korisnik> Korisnici { get; set; } = [];
        public List<Vozilo> Vozila { get; set; } = [];
        public List<RacunZaUslugu> Racuni { get; set; } = [];

        public TabeleBazaPodataka() { }
    }
}
