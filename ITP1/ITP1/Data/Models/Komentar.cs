using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITP1.Data.Models
{
    public class Komentar
    {
        public int Id { get; set; }
        public string Tekst { get; set; }
        public DateTime dateTime { get; set; }

        public int NekretninaId { get; set; }
        public int KorisnikId { get; set; }//ovlastenja

        public Korisnik Korisnik { get; set; }
    }
}
