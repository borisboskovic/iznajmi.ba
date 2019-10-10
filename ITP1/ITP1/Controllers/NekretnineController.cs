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

namespace ITP1.Controllers
{
    public class NekretnineController : Controller
    {
        private readonly INekretnina _nekretinina;
        private readonly NekretnineRepository _repo;

        public NekretnineController(INekretnina nekretnina, NekretnineRepository repo)
        {
            _nekretinina = nekretnina;
            _repo = repo;
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
            Nekretnina nekretnina = _nekretinina.GetNekretnina(id);
            return View(nekretnina);
        }

        [HttpPost]
        public IActionResult Edit(Nekretnina nekretnina)
        {
            _nekretinina.UpdateNekretnina(nekretnina);

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
                DateTime.TryParseExact(model.DostupnoOdString, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtOd);
                model.DostupnoOd = dtOd;
                DateTime.TryParseExact(model.DostupnoDoString, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtDo);
                model.DostupnoDo = dtDo == DateTime.MinValue ? DateTime.MaxValue : dtDo;

                await _nekretinina.AddNekretnina(model);
                return RedirectToAction("Index", "Home");
            }
            var mod = _repo.CreateNekretnina();
            mod.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View(mod);
        }


        
    }
}