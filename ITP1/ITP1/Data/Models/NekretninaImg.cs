using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITP1.Data.Models
{
    public class NekretninaImg
    {
        public int Id { get; set; }
        public String Url { get; set; }
        public String PublicId { get; set; }
        public bool IsCoverImg { get; set; }

        public int NekretninaId { get; set; }
        
        public Nekretnina Nekretnina { get; set; }
    }
}
