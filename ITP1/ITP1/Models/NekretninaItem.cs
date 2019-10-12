using ITP1.Data.Models;
using System;


namespace ITP1.Models
{
    public class NekretninaItem
    {
        public Korisnik Korisik { get; set; }
        public int Id { get; set; }
        public string Naslov { get; set; }
        public string Lokacija { get; set; }
        public double Cijena { get; set; }
        public double Povrsina { get; set; }
        public DateTime DostupnoOd { get; set; }
        public DateTime DostupnoDo { get; set; }
        public TipModel Tip { get; set; }
        public NacinIznajmljivanjaModel NacinIznajmljivanja { get; set; }
        public String CoverImgUrl { get; set; }
    }
}
