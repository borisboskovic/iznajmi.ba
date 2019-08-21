using ITP1.Data;
using ITP1.Data.Models;
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
    }
}
