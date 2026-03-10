using Domain.BazaPodataka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Database.BazaPodataka
{
    public class JsonBazaPodataka : IBazaPodataka
    {
        public TabeleBazaPodataka Tabele { get; set; }

        public JsonBazaPodataka()
        {
            try
            {
                if (File.Exists("podaci.json"))
                {
                    using StreamReader sr = new("podaci.json");
                    string json = sr.ReadToEnd();
                    Tabele = JsonConvert.DeserializeObject<TabeleBazaPodataka>(json) ?? new();
                }
                else
                    Tabele = new();
            }
            catch
            {
                Tabele = new();
            }
        }

        public bool SacuvajPromene()
        {
            try
            {
                string json = JsonConvert.SerializeObject(Tabele,
                              new JsonSerializerSettings
                              {
                                  ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                  Formatting = Formatting.Indented
                              });
                using StreamWriter sw = new("podaci.json");
                sw.Write(json);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}