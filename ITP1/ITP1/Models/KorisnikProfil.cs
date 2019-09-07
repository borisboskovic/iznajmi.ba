using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITP1.Models
{
    public class KorisnikProfil
    {
        public int Id { get; set; }
        public string CurrentUserId { get; set; }

        [Required(ErrorMessage ="Ovo polje je obavezno.")]
        [Display(Name = "Ime i prezime*")]
        public string Ime { get; set; }

        [Display(Name = "Telefon")]
        public string Tel { get; set; }

        [Display(Name = "E-pošta")]
        public string Mail { get; set; }
        public string AvatarImgUrl { get; set; }
        public IFormFile ImgFile { get; set; }

        [Display(Name = "Web")]
        public string WebKontaktUrl { get; set; }
        public double ProsjecnaOcjena { get; set; }
        public int BrojOcjena { get; set; }
        public string UserId { get; set; }
        public List<NekretninaItem> NekretninaItems { get; set; }
    }
}
