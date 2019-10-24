using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ITP1.Data;
using ITP1.Data.Models;
using ITP1.Models;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
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
                    ProsjecnaOcjena = Math.Round(_korisnik.GetProsjecnaOcjena(korisnikModel.Id), 1),
                    NekretninaItems = _korisnik.GetListaNekretninaZaKorisnika(korisnikModel.Id),
                    BrojOcjena = _korisnik.GetTotalNumberOcjena(korisnikModel.Id),
                    CurrentUserId = id,
                    Utisci = _korisnik.GetUtisci(korisnikModel.Id, korisnikModel.Id) == null ? new List<Utisak>() : _korisnik.GetUtisci(korisnikModel.Id, korisnikModel.Id),
                };

                return View(model);
            }
            return View("UnauthorizedAccess");
        }


        [HttpPost]
        [Authorize]
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
                return RedirectToAction("Edit", "Korisnik", new { id = korisnikModel.UserId });
            }

            return View("UnauthorizedAccess");
        }


        public IActionResult Details(string id)
        {
            int ocijenjeniKorinikId = _korisnik.GetKorisnikWithForeignKey(id).Id;
            Utisak lUtisak = new Utisak { Ocjena = 0, Komentar = "" };
            if (this.User.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                if(this.User.FindFirst(ClaimTypes.NameIdentifier).Value == id)
                    return RedirectToAction("Edit", "Korisnik", new { id = id});

                lUtisak = _korisnik.GetUtisak(_korisnik.GetKorisnikWithForeignKey(this.User.FindFirst(ClaimTypes.NameIdentifier).Value).Id, ocijenjeniKorinikId) == null ?
                    lUtisak : _korisnik.GetUtisak(_korisnik.GetKorisnikWithForeignKey(this.User.FindFirst(ClaimTypes.NameIdentifier).Value).Id, ocijenjeniKorinikId);

                
            }
            int loggedUserId = this.User.FindFirst(ClaimTypes.NameIdentifier) == null ? 0 : _korisnik.GetKorisnikWithForeignKey(this.User.FindFirst(ClaimTypes.NameIdentifier).Value).Id;
            var korisnikModel = _korisnik.GetKorisnikWithForeignKey(id);
            var model = new KorisnikProfil()
            {
                Id = korisnikModel.Id,
                StrId = id,
                UserId = korisnikModel.UserId,
                Ime = korisnikModel.Ime,
                AvatarImgUrl = korisnikModel.AvatarImgUrl,
                Tel = korisnikModel.Tel == null ? "" : korisnikModel.Tel,
                Mail = korisnikModel.MailKontakt == null ? "" : korisnikModel.MailKontakt,
                WebKontaktUrl = korisnikModel.WebKontaktUrl,
                ProsjecnaOcjena = Math.Round(_korisnik.GetProsjecnaOcjena(ocijenjeniKorinikId), 1),
                NekretninaItems = _korisnik.GetListaNekretninaZaKorisnika(korisnikModel.Id),
                BrojOcjena = _korisnik.GetTotalNumberOcjena(ocijenjeniKorinikId),
                LicniUtisak = lUtisak,
                Utisci = _korisnik.GetUtisci(_korisnik.GetKorisnikWithForeignKey(id).Id, loggedUserId) == null ? new List<Utisak>() : _korisnik.GetUtisci(_korisnik.GetKorisnikWithForeignKey(id).Id, loggedUserId),
            };

            return View(model);

        }

        [Authorize]
        [HttpPost]
        public IActionResult Details(KorisnikProfil model)
        {
            if (model.LicniUtisak.Ocjena != 0)
            {
                model.LicniUtisak.OcjenjeniKorinsnikid = model.Id;
                model.LicniUtisak.KorisnikId  = _korisnik.GetKorisnikWithForeignKey(this.User.FindFirst(ClaimTypes.NameIdentifier).Value).Id;
                _korisnik.AddUtisak(model.LicniUtisak);
            }

            return Details(model.StrId);
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