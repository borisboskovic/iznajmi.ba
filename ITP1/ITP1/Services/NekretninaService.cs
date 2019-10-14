using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ITP1.Data;
using ITP1.Data.Models;
using ITP1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public async Task AddNekretnina(NekretninaInsertModel insertModel)
        {
            var tip = _context.Tipovi.Where(t => t.Id == insertModel.TipId).FirstOrDefault();
            var nacinIznajmljivanja = _context.NacinIznajmljivnja.Where(n => n.Id == insertModel.NacinIznajmljivanjaId).FirstOrDefault();
            var korisnik = _context.Korisnici.Where(kor => kor.UserId == insertModel.UserId).FirstOrDefault();
            Marker marker = new Marker()
            {
                Lat = double.Parse(insertModel.Latitude, CultureInfo.InvariantCulture),
                Lng = double.Parse(insertModel.Longitude, CultureInfo.InvariantCulture)
            };
            _context.Markeri.Add(marker);
            Nekretnina nekretnina = new Nekretnina()
            {
                Naslov = insertModel.Naslov,
                Lokacija = insertModel.Lokacija,
                Cijena = insertModel.Cijena,
                Povrsina = insertModel.Povrsina,
                DostupnoOd = insertModel.DostupnoOd ?? DateTime.MinValue,
                DostupnoDo = insertModel.DostupnoDo ?? DateTime.MaxValue,
                Opis = insertModel.Opis,
                NacinIznajmljivanja=nacinIznajmljivanja,
                Tip=tip,
                Marker=marker,
                Korisnik=korisnik
            };
            _context.Nekretnine.Add(nekretnina);
            await _context.SaveChangesAsync();

            if (insertModel.ImgFiles != null)
            {
               await AddNekreninaImg(insertModel.ImgFiles, nekretnina.Id);
            }
        }

        private async Task AddNekreninaImg(List<IFormFile> imgs, int nekretninaId)
        {
            List<NekretninaImg> nekretnineImgs = new List<NekretninaImg>();
            for (int i = 0; i < imgs.Count; i++)
            {
                ImageUploadResult imgUplResults = await UpdateImgToCloudWithStreamAsync(imgs.ElementAt(i), nekretninaId, i.ToString());
                if (imgUplResults != null)
                {
                    NekretninaImg nekretninaImg = new NekretninaImg()
                    {
                        NekretninaId = nekretninaId,
                        Url = imgUplResults.Uri.ToString(),
                        PublicId = imgUplResults.PublicId,
                        IsCoverImg = i == 0 ? true : false,
                    };

                    //Neće add range iz nekog razloga
                    String query = "Insert into NekretninaImgs (Url, PublicId, NekretninaId, IsCoverImg) Values('" + nekretninaImg.Url + "', '" + nekretninaImg.PublicId + "', "+nekretninaImg.NekretninaId+", " + (nekretninaImg.IsCoverImg == true ? 1 : 0) + ")";
#pragma warning disable EF1000 // Possible SQL injection vulnerability.
                    _context.Database.ExecuteSqlCommand(query);
#pragma warning restore EF1000 // Possible SQL injection vulnerability.
                    await _context.SaveChangesAsync();

                }
            }
        }

        public async Task<ImageUploadResult> UpdateImgToCloudWithStreamAsync(IFormFile formFile, int nekretninaId, string imgName)
        {
            Account account = new Account(
                  "dysckfx4z",
                  "146882857231366",
                  "dOyF8Ue2EPJuNh73agNVjzKsxNk");

            Cloudinary cloudinary = new Cloudinary(account);

            if (formFile.Length > 0)
            {
                using (var stream = formFile.OpenReadStream())
                {

                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(formFile.FileName, stream),
                        PublicId = @"Nekretnine/" + nekretninaId + "/" + imgName,
                        Transformation = new Transformation().Width(400).Height(400).Crop("limit"),
                    };
                    var uploadResult = await cloudinary.UploadAsync(uploadParams);
                    return uploadResult;
                }
            }
            return null;
        }

        public Nekretnina GetNekretnina(int id)
        {
            return _context.Nekretnine.FirstOrDefault(nk => nk.Id == id);
        }

        public NekretninaUpadeModel GetNekretninaUpadeModel(int id)
        {
            NekretninaUpadeModel nekretninaUpadeModel = new NekretninaUpadeModel
            {
                Cijena = _context.Nekretnine.FirstOrDefault(nk => nk.Id == id).Cijena,
                DostupnoDo = _context.Nekretnine.FirstOrDefault(nk => nk.Id == id).DostupnoDo,
                DostupnoDoString = _context.Nekretnine.FirstOrDefault(nk => nk.Id == id).DostupnoDo.ToString(),
                DostupnoOd = _context.Nekretnine.FirstOrDefault(nk => nk.Id == id).DostupnoOd,
                DostupnoOdString = _context.Nekretnine.FirstOrDefault(nk => nk.Id == id).DostupnoOd.ToString(),
                Id = id,
                Longitude = Convert.ToDouble(_context.Nekretnine.Include(m => m.Marker).FirstOrDefault(nk => nk.Id == id).Marker.Lng).ToString(),
                Latitude = Convert.ToDouble(_context.Nekretnine.Include(m => m.Marker).FirstOrDefault(nk => nk.Id == id).Marker.Lat).ToString(),
                Opis = _context.Nekretnine.FirstOrDefault(nk => nk.Id == id).Opis,
                Naslov = _context.Nekretnine.FirstOrDefault(nk => nk.Id == id).Naslov,
                Lokacija = _context.Nekretnine.FirstOrDefault(nk => nk.Id == id).Lokacija
            };
            return nekretninaUpadeModel;
        }
        public void UpdateNekretnina(NekretninaUpadeModel nekretnina)
        {
            Double Lat = Convert.ToDouble(nekretnina.Latitude);
            Double Lng = Convert.ToDouble(nekretnina.Longitude);

            //_context.Nekretnine.FirstOrDefault(nk => nk.Id == nekretnina.Id).KorisnikId = nekretnina.KorisnikId;
            _context.Nekretnine.FirstOrDefault(nk => nk.Id == nekretnina.Id).Lokacija = nekretnina.Lokacija;
            _context.Nekretnine.Include(m => m.Marker).FirstOrDefault(nk => nk.Id == nekretnina.Id).Marker.Lat = Lat;
            _context.Nekretnine.Include(m => m.Marker).FirstOrDefault(nk => nk.Id == nekretnina.Id).Marker.Lng = Lng;
            //_context.Nekretnine.FirstOrDefault(nk => nk.Id == nekretnina.Id).MarkerId = nekretnina.MarkerId;
            _context.Nekretnine.FirstOrDefault(nk => nk.Id == nekretnina.Id).Naslov = nekretnina.Naslov;
            _context.Nekretnine.FirstOrDefault(nk => nk.Id == nekretnina.Id).Opis = nekretnina.Opis;
            _context.Nekretnine.FirstOrDefault(nk => nk.Id == nekretnina.Id).Povrsina = nekretnina.Povrsina;
            _context.Nekretnine.FirstOrDefault(nk => nk.Id == nekretnina.Id).Cijena = nekretnina.Cijena;
            _context.Nekretnine.FirstOrDefault(nk => nk.Id == nekretnina.Id).DostupnoDo = Convert.ToDateTime(nekretnina.DostupnoDoString);
            _context.Nekretnine.FirstOrDefault(nk => nk.Id == nekretnina.Id).DostupnoOd = Convert.ToDateTime(nekretnina.DostupnoOdString);
            //_context.Nekretnine.FirstOrDefault(nk => nk.Id == nekretnina.Id).TipId = nekretnina.TipId;

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
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoOd) >= 0 && DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoDo) <= 0 && DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
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
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoOd) >= 0 && DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoDo) <= 0 && DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
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
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoOd) >= 0 && DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoDo) <= 0 && DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
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
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoOd) >= 0 && DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoDo) <= 0 && DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
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
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoOd) >= 0 && DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoDo) <= 0 && DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
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
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoOd) >= 0 && DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoDo) <= 0 && DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Count();
            }
            else if (pModel.Filter.PovrsinaMax == 0)
            {
                return _context.Nekretnine.Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin && n.Cijena <= pModel.Filter.CijenaMax)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoOd) >= 0 && DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoDo) <= 0 && DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
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
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoOd) >= 0 && DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoDo) <= 0 && DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
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

        public void DeleteNekretnina(int id)
        {
            Nekretnina nekretnina = _context.Nekretnine.FirstOrDefault(nk => nk.Id == id);
            _context.Nekretnine.Remove(nekretnina);

            _context.SaveChanges();


        }
    }
}
