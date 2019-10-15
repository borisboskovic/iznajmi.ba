using System;
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

        [HttpGet]
        public IActionResult Display(int id)
        {
            Nekretnina nekretnina = _nekretinina.GetNekretnina(id);
            return View(nekretnina);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            NekretninaUpadeModel nekretnina = _nekretinina.GetNekretninaUpadeModel(id);
            return View(nekretnina);
        }

        [HttpPost]
        public IActionResult Edit(NekretninaUpadeModel nekretnina)
        {
            _nekretinina.UpdateNekretnina(nekretnina);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult DeleteNekretnina(int id)
        {
            _nekretinina.DeleteNekretnina(id);
            return RedirectToAction("Index", "Home");
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

        //Iz nekog razloga ovo uvijek vraća false no lo se!!!
        //[Authorize(Roles = "Admin")] 
        public async Task<IActionResult> DeleteKomentarDetails(int id, int nekretninaid)
        {
            if (await IsCurrentUserInRoleAsync("Admin"))
            {
                _nekretinina.DeleteKomentar(id);
                return RedirectToAction("Details", "Nekretnine", new { id = nekretninaid });
            }
            else
                return null;
            //Napravit svoj error page za unauthorized
        }

        public async Task<IActionResult> DeleteNekretninaDetails(int nekretninaid)
        {
            if (await IsCurrentUserInRoleAsync("Admin"))
            {
                await _nekretinina.DeleteNekretninaAsync(nekretninaid);
                return RedirectToAction("Index", "Home");
            }
            else
                return null;
            //Napravit svoj error page za unauthorized
        }

        //TODO obrisat,prebacit u edit, autorizacijaaaaaaa
        //[HttpPost]
        //public IActionResult Details(NekretninaImg model, string delete_btn, string set_as_cover_btn, int imgId)
        //{
        //    //iz nekod razloga id rutira id NekretniaDetail modela,a ne ovaj
        //    model.Id = imgId;
        //    if (delete_btn != null)
        //    {
        //        _nekretinina.DeleteImgAsync(model);
        //    }
        //    else if (set_as_cover_btn != null)
        //    {
        //        _nekretinina.SetNewCoverImg(model);
        //    }
        //    return Details(model.NekretninaId);
        //}

        //public async Task<IActionResult> InsertImgAsync(NekretninaDetails model)
        //{
        //    if(model.NewImgFile != null && model.Id != 0)
        //       await _nekretinina.AddNekretninaImg(model.NewImgFile, model.Id);

        //    return Details(model.Id);
        //}


        private async Task<bool> IsCurrentUserInRoleAsync(string role)
        {
            var user = await _userManager.FindByIdAsync(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
                return true;

            return false;
        }
        public IActionResult MapView()
        {
            return View();
        }
        
    }
}