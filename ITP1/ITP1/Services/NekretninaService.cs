using ITP1.Data;
using ITP1.Data.Models;
using ITP1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITP1.Services
{
    public class NekretninaService : INekretnina
    {
        private ApplicationDbContext _context;
        public NekretninaService(ApplicationDbContext context)
        {
            _context = context;
        }
        public void AddNekretnina(Nekretnina nekretnina)
        {
            _context.Nekretnine.Add(nekretnina);
            _context.SaveChanges();
        }

        public Nekretnina GetNekretnina(int id)
        {
            return _context.Nekretnine.FirstOrDefault(nk => nk.Id == id);
        }

        public void UpdateNekretnina(Nekretnina nekretnina)
        {
            _context.Nekretnine.FirstOrDefault(nk => nk.Id == nekretnina.Id).KorisnikId = nekretnina.KorisnikId;
            _context.Nekretnine.FirstOrDefault(nk => nk.Id == nekretnina.Id).Lokacija = nekretnina.Lokacija;
            _context.Nekretnine.FirstOrDefault(nk => nk.Id == nekretnina.Id).MarkerId = nekretnina.MarkerId;
            _context.Nekretnine.FirstOrDefault(nk => nk.Id == nekretnina.Id).Naslov = nekretnina.Naslov;
            _context.Nekretnine.FirstOrDefault(nk => nk.Id == nekretnina.Id).Opis = nekretnina.Opis;
            _context.Nekretnine.FirstOrDefault(nk => nk.Id == nekretnina.Id).Povrsina = nekretnina.Povrsina;
            _context.Nekretnine.FirstOrDefault(nk => nk.Id == nekretnina.Id).TipId = nekretnina.TipId;

            _context.SaveChanges();
        }

        public IEnumerable<Nekretnina> GetNekretnine(int pagenumber, int pagesize)
        {

            IEnumerable<Nekretnina> nekretnine = _context.Nekretnine
                .Include(k => k.Korisnik)
                .Include(m => m.Marker)
                .Include(t => t.Tip)
                .Include(ni => ni.NacinIznajmljivanja)
                .Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();

            nekretnine = AddCoverImg(nekretnine);
            return nekretnine;
        }


        private IEnumerable<Nekretnina> AddCoverImg(IEnumerable<Nekretnina> nekretninas)
        {
            foreach (var item in nekretninas)
            {
                item.CoverImg = _context.NekretninaImgs
                    .Where(ni => ni.NekretninaId == item.Id && ni.IsCoverImg == true)
                    .FirstOrDefault();
            }
            return nekretninas;
        }

        public int CountNekretnine()
        {
            return _context.Nekretnine.Count();
        }

        public IEnumerable<Nekretnina> GetAllNekretnineWithFilters(int pagenumber, int pagesize, PocetnaModel pModel)
        {
            List<int> tipoviCheckedIds = new List<int>();
            foreach (var item in pModel.SviTipovi)
            {
                if (item.Selected)
                    tipoviCheckedIds.Add(item.Id);
            }
            List<int> nacinIznIds = new List<int>();
            foreach (var item in pModel.NaciniIznajmljivanja)
            {
                if (item.Selected)
                    nacinIznIds.Add(item.Id);
            }
            IEnumerable<Nekretnina> nekretnine;

            if (pModel.Filter.CijenaMax == 0 && pModel.Filter.PovrsinaMax == 0 && pModel.Filter.DostupnoOd == DateTime.MinValue)
            {
                nekretnine = _context.Nekretnine.Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Include(k => k.Korisnik)
                                    .Include(m => m.Marker)
                                    .Include(t => t.Tip)
                                    .Include(ni => ni.NacinIznajmljivanja)
                                    .Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
            }
            else if (pModel.Filter.CijenaMax == 0 && pModel.Filter.PovrsinaMax == 0)
            {
                nekretnine = _context.Nekretnine.Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoOd) >= 0 && DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Include(k => k.Korisnik)
                                   .Include(m => m.Marker)
                                   .Include(t => t.Tip)
                                   .Include(ni => ni.NacinIznajmljivanja)
                                   .Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
            }
            else if (pModel.Filter.CijenaMax == 0 && pModel.Filter.DostupnoOd == DateTime.MinValue)
            {
                nekretnine = _context.Nekretnine.Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin && n.Povrsina <= pModel.Filter.PovrsinaMax)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Include(k => k.Korisnik)
                                    .Include(m => m.Marker)
                                    .Include(t => t.Tip)
                                    .Include(ni => ni.NacinIznajmljivanja)
                                    .Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
            }
            else if (pModel.Filter.PovrsinaMax == 0 && pModel.Filter.DostupnoOd == DateTime.MinValue)
            {
                nekretnine = _context.Nekretnine.Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin && n.Cijena <= pModel.Filter.CijenaMax)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Include(k => k.Korisnik)
                                    .Include(m => m.Marker)
                                    .Include(t => t.Tip)
                                    .Include(ni => ni.NacinIznajmljivanja)
                                    .Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
            }
            else if (pModel.Filter.CijenaMax == 0)
            {
                nekretnine = _context.Nekretnine.Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin && n.Povrsina <= pModel.Filter.PovrsinaMax)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoOd) >= 0 && DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Include(k => k.Korisnik)
                                    .Include(m => m.Marker)
                                    .Include(t => t.Tip)
                                    .Include(ni => ni.NacinIznajmljivanja)
                                    .Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
            }
            else if (pModel.Filter.PovrsinaMax == 0)
            {
                nekretnine = _context.Nekretnine.Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin && n.Cijena <= pModel.Filter.CijenaMax)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoOd) >= 0 && DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Include(k => k.Korisnik)
                                    .Include(m => m.Marker)
                                    .Include(t => t.Tip)
                                    .Include(ni => ni.NacinIznajmljivanja)
                                    .Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
            }
            else if (pModel.Filter.DostupnoOd == DateTime.MinValue)
            {
                nekretnine = _context.Nekretnine.Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin && n.Povrsina <= pModel.Filter.PovrsinaMax)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin && n.Cijena <= pModel.Filter.CijenaMax)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Include(k => k.Korisnik)
                                    .Include(m => m.Marker)
                                    .Include(t => t.Tip)
                                    .Include(ni => ni.NacinIznajmljivanja)
                                    .Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
            }
            else
            {
                nekretnine = _context.Nekretnine.Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin && n.Povrsina <= pModel.Filter.PovrsinaMax)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin && n.Cijena <= pModel.Filter.CijenaMax)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoOd) >= 0 && DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Include(k => k.Korisnik)
                                    .Include(m => m.Marker)
                                    .Include(t => t.Tip)
                                    .Include(ni => ni.NacinIznajmljivanja)
                                    .Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
            }

            nekretnine = AddCoverImg(nekretnine);
            return nekretnine;
        }


        public int CountNekretnineWithFilters(PocetnaModel pModel)
        {
            List<int> tipoviCheckedIds = new List<int>();
            foreach (var item in pModel.SviTipovi)
            {
                if (item.Selected)
                    tipoviCheckedIds.Add(item.Id);
            }
            List<int> nacinIznIds = new List<int>();
            foreach (var item in pModel.NaciniIznajmljivanja)
            {
                if (item.Selected)
                    nacinIznIds.Add(item.Id);
            }
            if (pModel.Filter.CijenaMax == 0 && pModel.Filter.PovrsinaMax == 0 && pModel.Filter.DostupnoOd == DateTime.MinValue)
            {
                return _context.Nekretnine.Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Count();
            }
            else if (pModel.Filter.CijenaMax == 0 && pModel.Filter.PovrsinaMax == 0)
            {
                return _context.Nekretnine.Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoOd) >= 0 && DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Count();
            }
            else if (pModel.Filter.CijenaMax == 0 && pModel.Filter.DostupnoOd == DateTime.MinValue)
            {
                return _context.Nekretnine.Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin && n.Povrsina <= pModel.Filter.PovrsinaMax)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Count();
            }
            else if (pModel.Filter.PovrsinaMax == 0 && pModel.Filter.DostupnoOd == DateTime.MinValue)
            {
                return _context.Nekretnine.Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin && n.Cijena <= pModel.Filter.CijenaMax)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Count();
            }
            else if (pModel.Filter.CijenaMax == 0)
            {
                return _context.Nekretnine.Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin && n.Povrsina <= pModel.Filter.PovrsinaMax)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoOd) >= 0 && DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Count();
            }
            else if (pModel.Filter.PovrsinaMax == 0)
            {
                return _context.Nekretnine.Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin && n.Cijena <= pModel.Filter.CijenaMax)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoOd) >= 0 && DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Count();
            }
            else if (pModel.Filter.DostupnoOd == DateTime.MinValue)
            {
                return _context.Nekretnine.Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin && n.Povrsina <= pModel.Filter.PovrsinaMax)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin && n.Cijena <= pModel.Filter.CijenaMax)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Count();
            }
            else
            {
                return _context.Nekretnine.Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin && n.Povrsina <= pModel.Filter.PovrsinaMax)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin && n.Cijena <= pModel.Filter.CijenaMax)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoOd) >= 0 && DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Count();
            }
        }

        public int CountNekretnineWithSearch(IEnumerable<Nekretnina> nekretninas, String searchString)
        {
            return nekretninas.Where(n => n.Naslov.ToLower().Contains(searchString.ToLower()) || n.Lokacija.ToLower().Contains(searchString.ToLower()))
                .Count();
        }

        public IEnumerable<Nekretnina> SearchNekretnine(IEnumerable<Nekretnina> nekretninas, String searchString)
        {
            return nekretninas.Where(n => n.Naslov.ToLower().Contains(searchString.ToLower()) || n.Lokacija.ToLower().Contains(searchString.ToLower()))
                .ToList();
        }

        public IEnumerable<Tip> GetAllTipoviFiltera()
        {
            return _context.Tipovi;
        }

        public IEnumerable<NacinIznajmljivanja> GetAllNaciniIznajmljivanja()
        {
            return _context.NacinIznajmljivnja;
        }

    }
}
