using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Enumeracije;

namespace Domain.Modeli
{
    public class Vozilo
    {
        public long Id { get; set; } = 0;
        public string RegistarskiBroj { get; set; } = string.Empty;
        public string Marka { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public TipVozila Tip { get; set; }
        public decimal ProcenjenaCenaUsluge { get; set; } = 0;
        public bool Servisirano { get; set; } = false;

        public Vozilo() { }

        public Vozilo(string registarskiBroj, string marka, string model, TipVozila tip, decimal procenjenaCena)
        {
            RegistarskiBroj = registarskiBroj;
            Marka = marka;
            Model = model;
            Tip = tip;
            ProcenjenaCenaUsluge = procenjenaCena;
            Servisirano = false;
        }

        public static string Header()
        {
            return $"| {"Reg. broj",-12} | {"Marka",-12} | {"Model",-12} | {"Tip",-10} | {"Cena(RSD)",-12} | {"Servisiran",-10} |" +
                   "\n" + new string('-', 80);
        }

        public override string ToString()
        {
            return $"| {RegistarskiBroj,-12} | {Marka,-12} | {Model,-12} | {Tip,-10} | {ProcenjenaCenaUsluge,-12:F2} | {(Servisirano ? "DA" : "NE"),-10} |";
        }
    }
}
