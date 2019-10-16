using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITP1.Models
{
    public class KorisnikAdministracijaModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Ime { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
