using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ITP1.Data;
using ITP1.Data.Models;
using ITP1.Models;
using Microsoft.AspNetCore.Mvc;

namespace ITP1.Controllers
{
    public class KorisnikController : Controller
    {
        private readonly IKorisnik _korisnik;
        public KorisnikController(IKorisnik korisnik)
        {
            _korisnik = korisnik;
        }


        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (IsAuthorized(id))
            {
                var korisnikModel = _korisnik.GetKorisnikWithForeignKey(id);

                var model = new KorisnikProfil()
                {
                    Id = korisnikModel.Id,
                    UserId = korisnikModel.UserId,
                    Ime = korisnikModel.Ime,
                    AvatarImgUrl = korisnikModel.AvatarImgUrl,
                    Tel = korisnikModel.Tel == null ? "" : korisnikModel.Tel,
                    Mail = korisnikModel.MailKontakt == null ? "" : korisnikModel.MailKontakt,
                    WebKontaktUrl = korisnikModel.WebKontaktUrl,
                    ProsjecnaOcjena = _korisnik.GetProsjecnaOcjena(korisnikModel.NumberOfRatings1, korisnikModel.NumberOfRatings2, korisnikModel.NumberOfRatings3, korisnikModel.NumberOfRatings4, korisnikModel.NumberOfRatings5),
                    NekretninaItems = _korisnik.GetListaNekretninaZaKorisnika(korisnikModel.Id),
                    BrojOcjena = korisnikModel.NumberOfRatings1 + korisnikModel.NumberOfRatings2 + korisnikModel.NumberOfRatings3 + korisnikModel.NumberOfRatings4 + korisnikModel.NumberOfRatings5,
                    CurrentUserId = id,
                };

                return View(model);
            }

            //Napraviti svoj error Posle
            return View();//???? error page*/
        }

        [HttpPost]
        public async Task<IActionResult> Edit(KorisnikProfil korisnikModel)
        {
            if (ModelState.IsValid && IsAuthorized(korisnikModel.CurrentUserId))
            {
                string avatar = null;
                if (korisnikModel.ImgFile != null)
                {
                    avatar = await _korisnik.UpdateUserImgToCloudWithStreamAsync(korisnikModel.ImgFile, korisnikModel.UserId, korisnikModel.Id);
                }

                var korisnik = new Korisnik()
                {
                    Id = korisnikModel.Id,
                    UserId = korisnikModel.UserId,
                    Ime = korisnikModel.Ime,
                    MailKontakt = korisnikModel.Mail,
                    Tel = korisnikModel.Tel,
                    AvatarImgUrl = avatar == null ? _korisnik.GetAvatarImgUrl(korisnikModel.Id) : avatar,
                    WebKontaktUrl = korisnikModel.WebKontaktUrl,
                };
                _korisnik.UpdateKorisnik(korisnik);
                return RedirectToAction("Index", "Home", new { area = "" });
                //return View(korisnikModel);
            }

            return View(korisnikModel);
        }



        protected Boolean IsAuthorized(string id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userId.ToString() == id)
            {
                return true;
            }
            return false;
        }
    }
}