using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITP1.Models
{
    public class AdministracijaModel
    {
        public List<KorisnikAdministracijaModel> Korisnici { get; set; }
        public int CurrPage { get; set; }
        public Pager Pager { get; set; }
        public String SearchString { get; set; }
    }
}
