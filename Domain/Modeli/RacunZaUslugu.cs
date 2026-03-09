using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Modeli
{
    public class RacunZaUslugu
    {
        public long Id { get; set; } = 0;
        public string ImeMehanicara { get; set; } = string.Empty;
        public DateTime DatumVremeIzdavanja { get; set; }
        public decimal UkupanIznos { get; set; } = 0;
        public long VoziloId { get; set; } = 0;

        public RacunZaUslugu() { }

        public RacunZaUslugu(string imeMehanicara, DateTime datumVreme, decimal ukupanIznos, long voziloId)
        {
            ImeMehanicara = imeMehanicara;
            DatumVremeIzdavanja = datumVreme;
            UkupanIznos = ukupanIznos;
            VoziloId = voziloId;
        }

        public static string Header()
        {
            return $"| {"Mehaničar",-20} | {"Datum i vreme",-20} | {"Iznos(RSD)",-12} | {"VoziloId",-12} |" +
                   "\n" + new string('-', 75);
        }

        public override string ToString()
        {
            return $"| {ImeMehanicara,-20} | {DatumVremeIzdavanja:dd.MM.yyyy HH:mm,-20} | {UkupanIznos,-12:F2} | {VoziloId,-12} |";
        }
    }
}