using ITP1.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITP1.Models
{
    public class NekretninaUpadeModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [Display(Name = "Naziv nekretnine")]
        public string Naslov { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [Display(Name = "Adresa")]
        public string Lokacija { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        public int Cijena { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [Display(Name = "Površina")]
        public int Povrsina { get; set; }



        public string Opis { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [Display(Name = "Tip nekretnine")]
        public int TipId { get; set; }
        //public IEnumerable<SelectListItem> Tipovi { get; set; }
        public string Tip { get; set; }
        public SelectList Tipovi { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [Display(Name = "Nacin iznajmljivanja nekretnine")]
        public int NacinIznajmljivanjaId { get; set; }
        //public IEnumerable<SelectListItem> NaciniIznajmljivanja { get; set; }
        [Display(Name = "Način iznajmljivanja")]
        public String NacinIznajmljivanja { get; set; }
        [Display(Name = "Način iznajmljivanja")]
        public SelectList NaciniIznajmljivanja { get; set; }

        public List<IFormFile> ImgFiles { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        public String Latitude { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        public String Longitude { get; set; }

        public string UserId { get; set; }


        [Display(Name = "Dostupno od")]
        public DateTime? DostupnoOd { get; set; }

        [Display(Name = "Dostupno do")]
        public DateTime? DostupnoDo { get; set; }

        [Display(Name = "Dostupno od")]
        public String DostupnoOdString { get; set; }
        [Display(Name = "Dostupno do")]
        public String DostupnoDoString { get; set; }


        public String CoverImgUrl { get; set; }
        public List<NekretninaImg> Imgs { get; set; }
        public IFormFile NewImgFile { get; set; }//Ovo za edit
    }
}

