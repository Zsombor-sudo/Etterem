using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etterem
{
    delegate void RendelesTeljesitesKezelo(string etelNeve);
    delegate void HozzavaloSzuksegesKezelo(string hozzavalo);
    delegate void HozzavaloElkeszultKezelo(string hozzavalo);
    class Etel
    {
        string megnevezes;
        string[] hozzavalok;

        public string Megnevezes { get { return megnevezes; } }
        public string[] Hozzavalok { get { return hozzavalok; } }

        public Etel(string megnevezes, string[] hozzavalok)
        {
            this.megnevezes = megnevezes;
            this.hozzavalok = hozzavalok;
        }

        
    }

    class Sef
    {

        Etel[] receptek = new Etel[]
        {
            new Etel("poharviz", new string[] { "viz" } ),
            new Etel("leves", new string[] { "repa", "hus", "krumpli", "viz" } ),
            new Etel("rantothus", new string[] { "hus", "krumpli" } ),
            new Etel("fozelek", new string[] { "viz", "repa" } )
        };
        public RendelesTeljesitesKezelo RendelesTeljesitve;
        public RendelesTeljesitesKezelo RendelesNemTeljesitheto;
        HozzavaloSzuksegesKezelo HozzavaloSzukseges;
        private Etel cel;
        private int szuksegesHozzavaloSzam;
        public void Megrendeles(string etelNeve)
        {
            Console.WriteLine($"Séf: Rendelés beérkezett '{etelNeve}'");
            for (int i = 0; i < receptek.Length; i++)
            {
                if (receptek[i].Megnevezes == etelNeve)
                {
                    Elkeszites(receptek[i]);
                }
                else
                {
                    RendelesNemTeljesitheto(etelNeve);
                }
            }
        }
        
        public void Elkeszites(Etel recept)
        {
            cel = recept;
            szuksegesHozzavaloSzam = recept.Hozzavalok.Count();
            foreach (var item in recept.Hozzavalok)
            {
                HozzavaloSzukseges(item);
            }
        }

        public void SzakacsElkeszult(string hozzavalo)
        {
            szuksegesHozzavaloSzam--;
            if (szuksegesHozzavaloSzam == 0)
            {
                Console.WriteLine($"Séf: Elkeszült a '{hozzavalo}'");
                RendelesTeljesitve(hozzavalo);
            }
        }

        public void Felvesz(Szakacs szakacs)
        {
            HozzavaloSzukseges += szakacs.SefKerValamit;
            szakacs.HozzavaloElkeszult += SzakacsElkeszult;
        }
    }
    class Szakacs
    {
        string nev;
        public string Nev { get { return nev; } }
        string specialitas;
        public Szakacs(string nev, string specialitas)
        {
            this.nev = nev;
            this.specialitas = specialitas;
        }

        public event HozzavaloElkeszultKezelo HozzavaloElkeszult;

        public void SefKerValamit(string hozzavalo)
        {
            if (hozzavalo == specialitas)
            {
                
                Foz();
            }
        }
        public void Foz()
        {
            HozzavaloElkeszult(specialitas);
        }
    }

    class Program
    {
        static void KiirSikertelen(string recept)
        {
            Console.WriteLine($"Sikertelen rendelés '{ recept }'");
        }
        static void KiirSikeres(string recept)
        {
            Console.WriteLine($"Sikeres rendelés '{ recept }'");
        }

        static void Main(string[] args)
        {
            Sef sef = new Sef();
            sef.RendelesTeljesitve += KiirSikeres;
            sef.RendelesNemTeljesitheto += KiirSikertelen;
            Szakacs bela = new Szakacs("Bela", "viz");
            sef.Felvesz(bela);
            sef.Megrendeles("poharviz");
            sef.Megrendeles("kecskesajt");
            Console.ReadKey();
        }
    }
}
