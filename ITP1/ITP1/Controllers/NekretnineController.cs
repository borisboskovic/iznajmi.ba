using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ITP1.Data;
using ITP1.Data.Models;

namespace ITP1.Controllers
{
    public class NekretnineController : Controller
    {
        private readonly INekretnina _nekretinina;

        public NekretnineController(INekretnina nekretnina)
        {
            _nekretinina = nekretnina;
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
    }
}