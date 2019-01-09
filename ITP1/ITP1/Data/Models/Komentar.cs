using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITP1.Data.Models
{
    public class Komentar
    {
        public int Id { get; set; }
        public string Tekst { get; set; }
        public DateTime dateTime { get; set; }

        public string NekretninaId { get; set; }
        public string KorisnikId { get; set; }//ovlastenja
    }
}
