using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ITP1.Data;
using ITP1.Data.Models;
using ITP1.Models;

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

        [HttpGet]
        public IActionResult Insert()
        {
            var model = _repo.CreateNekretnina();
            return View(model);
        }
    }
}