using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Enumeracije;
using Domain.Servisi;

namespace Services.LoggerServisi
{
    public class FileLoggerServis(string putanja = "autoservis_log.txt") : ILoggerServis
    {
        private string _putanja = putanja;

        public bool EvidentirajDogadjaj(TipEvidencije tip, string poruka)
        {
            try
            {
                using StreamWriter sw = new(_putanja, append: true);
                sw.Write($"[{tip}]: {DateTime.Now:dd.MM.yyyy HH:mm:ss} - {poruka}\n");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}