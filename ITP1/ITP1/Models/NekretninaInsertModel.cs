using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITP1.Models
{
    public class NekretninaInsertModel
    {
        [Display(Name ="Naziv nekretnine")]
        public string Naslov { get; set; }

        [Display(Name ="Adresa")]
        public string Lokacija { get; set; }

        [Required(ErrorMessage ="Obavezno polje")]
        public int Cijena { get; set; }

        [Required]
        public int Povrsina { get; set; }

        [Required]
        [Display(Name ="Dostupno od")]
        public DateTime DostupnoOd { get; set; }

        [Required]
        [Display(Name ="Dostupno do")]
        public DateTime DostupnoDo { get; set; }

        public string Opis { get; set; }

        [Required]
        [Display(Name ="Tip nekretnine")]
        public int TipId { get; set; }
        public IEnumerable<SelectListItem> Tipovi { get; set; }

        [Required]
        [Display(Name ="Nacin iznajmljivanja nekretnine")]
        public int NacinIznajmljivanjaId { get; set; }
        public IEnumerable<SelectListItem> NaciniIznajmljivanja { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string UserId { get; set; }
    }
}
