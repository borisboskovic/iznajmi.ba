using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITP1.Data.Models
{
    public class Utisak
    {
        public int Id { get; set; }
        public int KorisnikId { get; set; }
        public int OcjenjeniKorinsnikid { get; set; }
        public String Komentar { get; set; }
        public int Ocjena { get; set; }

        public Korisnik Korisnik { get; set; }
    }
}
