using ITP1.Data.Models;
using ITP1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITP1.Data
{
    public class NekretnineRepository
    {
        private readonly ApplicationDbContext _context;

        public NekretnineRepository(ApplicationDbContext context) => _context = context;
        //{
        //    _context = context;
        //}

        public NekretninaInsertModel CreateNekretnina()
        {
            List<SelectListItem> tipovi = _context.Tipovi.AsNoTracking().OrderBy(tip => tip.ImeTipa)
                .Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.ImeTipa
                }).ToList();
            var tipoviSelectList = new SelectList(tipovi, "Value", "Text");

            List<SelectListItem> naciniIznajmljivanja = _context.NacinIznajmljivnja.AsNoTracking().OrderBy(nac => nac.Naziv)
                .Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.Naziv
                }).ToList();
            var naciniIznajmljivanjaSelectList = new SelectList(naciniIznajmljivanja, "Value", "Text");

            var nekretnina = new NekretninaInsertModel();
            nekretnina.Tipovi = tipoviSelectList;
            nekretnina.NaciniIznajmljivanja = naciniIznajmljivanjaSelectList;

            return nekretnina;
        }
    }
}
