using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITP1.Services
{
    public class NekretninaListModel
    {
        public int Id { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Naslov { get; set; }
        public string Lokacija { get; set; }
        public string Tip { get; set; }
        public string NacinIznajmljivanja { get; set; }
        public double Cijena { get; set; }
        public int Povrsina { get; set; }
        public DateTime DostupnoOd { get; set; }
        public DateTime DostupnoDo { get; set; }
    }
}
