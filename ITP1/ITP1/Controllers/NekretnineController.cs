﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ITP1.Data;
using ITP1.Data.Models;
using ITP1.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace ITP1.Controllers
{
    public class NekretnineController : Controller
    {
        private readonly INekretnina _nekretinina;
        private readonly NekretnineRepository _repo;
        private readonly IKorisnik _korisnik;
        private readonly UserManager<IdentityUser> _userManager;


        public NekretnineController(INekretnina nekretnina, NekretnineRepository repo, IKorisnik korisnik, UserManager<IdentityUser> userManager)
        {
            _nekretinina = nekretnina;
            _repo = repo;
            _korisnik = korisnik;
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }


        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (IsAuthorizedUserForNekretnina(id))
            {
                NekretninaUpadeModel nekretnina = _nekretinina.GetNekretninaUpadeModel(id);
                return View(nekretnina);
            }
            return View("UnauthorizedAccess");
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(NekretninaUpadeModel nekretnina)
        {
            if (ModelState.IsValid && IsAuthorizedUserForNekretnina(nekretnina.Id))
            {
                _nekretinina.UpdateNekretnina(nekretnina);

                return RedirectToAction("Index", "Home");
            }

            return View("UnauthorizedAccess");

        }

        [Authorize]
        public async Task<IActionResult> DeleteNekretnina(int id)
        {
            if (IsAuthorizedUserForNekretnina(id) || await IsCurrentUserInRoleAsync("Admin"))
            {
                await _nekretinina.DeleteNekretninaAsync(id);
                return RedirectToAction("Index", "Home");
            }

            return View("UnauthorizedAccess");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Insert()
        {
                var model = _repo.CreateNekretnina();
                model.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Insert(NekretninaInsertModel model)
        {
            if (ModelState.IsValid)
            {
                DateTime dtOd = DateTime.MinValue;
                DateTime dtDo = DateTime.MaxValue;
                if(!DateTime.TryParseExact(model.DostupnoOdString, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtOd))
                    DateTime.TryParseExact(model.DostupnoOdString, "dd.MM.yyyy H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtOd);

                model.DostupnoOd = dtOd;
                if(!DateTime.TryParseExact(model.DostupnoDoString, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtDo))
                    DateTime.TryParseExact(model.DostupnoDoString, "dd.MM.yyyy H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtDo);

                model.DostupnoDo = dtDo == DateTime.MinValue ? DateTime.MaxValue : dtDo;

                await _nekretinina.AddNekretnina(model);
                return RedirectToAction("Index", "Home");
            }
            var mod = _repo.CreateNekretnina();
            mod.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View(mod);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            Nekretnina nekretnina = _nekretinina.GetNekretnina(id);
            NekretninaDetails nekretninaDetailsModel = new NekretninaDetails()
            {
                Id = nekretnina.Id,
                Naslov = nekretnina.Naslov,
                Cijena = nekretnina.Cijena,
                Povrsina = nekretnina.Povrsina,
                DostupnoOd = nekretnina.DostupnoOd,
                DostupnoDo = nekretnina.DostupnoDo,
                Lokacija = nekretnina.Lokacija,
                NacinIznajmljivanja = nekretnina.NacinIznajmljivanja,
                Marker = nekretnina.Marker,
                Tip = nekretnina.Tip,
                Korisnik = nekretnina.Korisnik,
                Imgs = _nekretinina.GetNekretnineImg(id) == null ? new List<NekretninaImg>() : _nekretinina.GetNekretnineImg(id),
                Opis = nekretnina.Opis,
                Komentari = _nekretinina.GetKomentariForNekretnina(id) == null ? new List<Komentar>() : _nekretinina.GetKomentariForNekretnina(id),
                BrojKomentara = _nekretinina.CountKomentari(id),
            };
            if (nekretninaDetailsModel.Imgs.Where(n => n.IsCoverImg == true).FirstOrDefault() != null)
                nekretninaDetailsModel.CoverImgUrl = nekretninaDetailsModel.Imgs.Where(n => n.IsCoverImg == true).FirstOrDefault().Url;
            return View(nekretninaDetailsModel);
        }


        [Authorize]
        [HttpPost]
        public IActionResult Details(Komentar newKomentar, int nekretninaId)
        {
            if (nekretninaId != 0)
            {
                newKomentar.KorisnikId = _korisnik.GetIdWithForeignKey(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                newKomentar.NekretninaId = nekretninaId;
                newKomentar.dateTime = DateTime.Now;
                _nekretinina.AddKomentar(newKomentar);
                return RedirectToAction("Details", "Nekretnine", new { id = nekretninaId });
            }

            return Details(nekretninaId);

        }

        [Authorize]
        public async Task<IActionResult> DeleteKomentarDetails(int id, int nekretninaid)
        {
            if (await IsCurrentUserInRoleAsync("Admin"))
            {
                _nekretinina.DeleteKomentar(id);
                return RedirectToAction("Details", "Nekretnine", new { id = nekretninaid });
            }

            return View("UnauthorizedAccess");
        }

        [Authorize]
        public async Task<IActionResult> DeleteNekretninaDetails(int nekretninaid)
        {
            if (IsAuthorizedUserForNekretnina(nekretninaid) || await IsCurrentUserInRoleAsync("Admin"))
            {
                await _nekretinina.DeleteNekretninaAsync(nekretninaid);
                return RedirectToAction("Index", "Home");
            }

            return View("UnauthorizedAccess");

        }

        [Authorize]
        [HttpPost]
        public IActionResult EditImg(NekretninaImg model, string delete_btn, string set_as_cover_btn, int imgId)
        {
            if (IsAuthorizedUserForNekretnina(model.Id))
            {
                model.Id = imgId;
                if (delete_btn != null)
                {
                    _nekretinina.DeleteImgAsync(model);
                }
                else if (set_as_cover_btn != null)
                {
                    _nekretinina.SetNewCoverImg(model);
                }
                return RedirectToAction("Edit", "Nekretnine", new { id = model.NekretninaId });
            }
            return View("UnauthorizedAccess");
        }

        [Authorize]
        public async Task<IActionResult> InsertImgAsync(IFormFile newImg, int model_id)
        {
            if (IsAuthorizedUserForNekretnina(model_id))
            {
                if (newImg != null && model_id != 0)
                    await _nekretinina.AddNekretninaImg(newImg, model_id);

                return RedirectToAction("Edit", "Nekretnine", new { id = model_id });
            }
            return View("UnauthorizedAccess");

        }


        private async Task<bool> IsCurrentUserInRoleAsync(string role)
        {
            var user = await _userManager.FindByIdAsync(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(role))
                return true;

            return false;
        }
        public IActionResult MapView(MapViewModel model)
        {
            if (model.NaciniIznajmljivanja == null)
                model = _repo.CreateMapViewModel();
            model = _nekretinina.GetNekretninasFiltered(model);
            return View(model);
        }

        private bool IsAuthorizedUserForNekretnina(int nekretninaId)
        {
            if (_nekretinina.GetUserIdFromNekretnina(nekretninaId) == this.User.FindFirst(ClaimTypes.NameIdentifier).Value)
                return true;

            return false;
        }

    }
}