using ITP1.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace ITP1.Models
{
    public class NekretninaDetails
    {
        public int Id { get; set; }
        public string Naslov { get; set; }
        public string Lokacija { get; set; }
        public double Cijena { get; set; }
        public double Povrsina { get; set; }
        public DateTime DostupnoOd { get; set; }
        public DateTime DostupnoDo { get; set; }
        public String Opis { get; set; }
        public Tip Tip { get; set; }
        public NacinIznajmljivanja NacinIznajmljivanja { get; set; }
        public String CoverImgUrl { get; set; }
        public Marker Marker { get; set; }
        public Korisnik Korisnik { get; set; }
        public List<NekretninaImg> Imgs { get; set; }
        public List<Komentar> Komentari { get; set; }
        public Komentar NewKomentar { get; set; }
        public int BrojKomentara { get; set; }


        public IFormFile NewImgFile { get; set; }//Ovo za edit
    }
}
