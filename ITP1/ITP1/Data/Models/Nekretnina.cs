using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITP1.Data.Models
{
    public class Nekretnina
    {

        public int Id { get; set; }
        public string Naslov { get; set; }
        public string Lokacija { get; set; }
        public int Cijena { get; set; }
        public int Povrsina { get; set; }
        public DateTime DostupnoOd { get; set; }
        public DateTime DostupnoDo { get; set; }
        public string Opis { get; set; }

        public int TipId { get; set; }
        public int KorisnikId { get; set; }
        public int MarkerId { get; set; }
        //public int NekretnidaImgId { get; set; }

       // public NekretninaImg Slike { get; set; }
        public Marker Marker { get; set; }
        public Tip Tip { get; set; }
        public Korisnik Korisnik { get; set; }
    }
}
