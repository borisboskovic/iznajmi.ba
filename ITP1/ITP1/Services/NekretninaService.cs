using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ITP1.Data;
using ITP1.Data.Models;
using ITP1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
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
                NacinIznajmljivanja = nacinIznajmljivanja,
                Tip = tip,
                Marker = marker,
                Korisnik = korisnik
            };
            _context.Nekretnine.Add(nekretnina);
            await _context.SaveChangesAsync();

            if (insertModel.ImgFiles != null)
            {
                await AddNekreninaImgs(insertModel.ImgFiles, nekretnina.Id);
            }
        }

        private async Task AddNekreninaImgs(List<IFormFile> imgs, int nekretninaId)
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
                    String query = "Insert into NekretninaImgs (Url, PublicId, NekretninaId, IsCoverImg) Values('" + nekretninaImg.Url + "', '" + nekretninaImg.PublicId + "', " + nekretninaImg.NekretninaId + ", " + (nekretninaImg.IsCoverImg == true ? 1 : 0) + ")";
#pragma warning disable EF1000 // Possible SQL injection vulnerability.
                    _context.Database.ExecuteSqlCommand(query);
#pragma warning restore EF1000 // Possible SQL injection vulnerability.
                    await _context.SaveChangesAsync();

                }
            }
        }

        public async Task AddNekretninaImg(IFormFile img, int nekretninaId)
        {
            ImageUploadResult imgUplResults = await UpdateImgToCloudWithStreamAsync(img, nekretninaId, GetNewImgName(nekretninaId));

            if (imgUplResults != null)
            {
                NekretninaImg nekretninaImg = new NekretninaImg()
                {
                    NekretninaId = nekretninaId,
                    Url = imgUplResults.Uri.ToString(),
                    PublicId = imgUplResults.PublicId,
                    IsCoverImg = false,
                };

                //Neće add range iz nekog razloga
                String query = "Insert into NekretninaImgs (Url, PublicId, NekretninaId, IsCoverImg) Values('" + nekretninaImg.Url + "', '" + nekretninaImg.PublicId + "', " + nekretninaImg.NekretninaId + ", " + (nekretninaImg.IsCoverImg == true ? 1 : 0) + ")";
#pragma warning disable EF1000 // Possible SQL injection vulnerability.
                _context.Database.ExecuteSqlCommand(query);
#pragma warning restore EF1000 // Possible SQL injection vulnerability.
                await _context.SaveChangesAsync();

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
            return _context.Nekretnine
                .Include(nk => nk.Korisnik)
                .Include(nk => nk.Marker)
                .Include(nk => nk.NacinIznajmljivanja)
                .Include(nk => nk.Tip)
                .FirstOrDefault(nk => nk.Id == id);
        }

        public List<NekretninaImg> GetNekretnineImg(int nekretnineId)
        {
            return _context.NekretninaImgs
                .Where(ni => ni.NekretninaId == nekretnineId)
                .ToList();
        }

        public NekretninaUpadeModel GetNekretninaUpadeModel(int id)
        {
            var nekretnina = _context.Nekretnine.Include(m => m.Marker).Include(n => n.Tip).Include(ni => ni.NacinIznajmljivanja).FirstOrDefault(nk => nk.Id == id);

            NekretninaUpadeModel nekretninaUpadeModel = new NekretninaUpadeModel
            {
                Cijena = nekretnina.Cijena,
                DostupnoDo = nekretnina.DostupnoDo,
                DostupnoDoString = nekretnina.DostupnoDo.ToString(),
                DostupnoOd = nekretnina.DostupnoOd,
                DostupnoOdString = nekretnina.DostupnoOd.ToString(),
                Id = id,
                Longitude = Convert.ToDouble(nekretnina.Marker.Lng).ToString(),
                Latitude = Convert.ToDouble(nekretnina.Marker.Lat).ToString(),
                Opis = nekretnina.Opis,
                Naslov = nekretnina.Naslov,
                Lokacija = nekretnina.Lokacija,

                Tipovi = GettTipoviSelectList(nekretnina.Tip.Id),
                NaciniIznajmljivanja = GettNaciniIznajmljivanjaSelectList(nekretnina.NacinIznajmljivanja.Id),
                Imgs = GetNekretnineImg(id) == null ? new List<NekretninaImg>() : GetNekretnineImg(id),
            };

            if (nekretninaUpadeModel.Imgs.Where(n => n.IsCoverImg == true).FirstOrDefault() != null)
                nekretninaUpadeModel.CoverImgUrl = nekretninaUpadeModel.Imgs.Where(n => n.IsCoverImg == true).FirstOrDefault().Url;

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
            _context.Nekretnine.FirstOrDefault(nk => nk.Id == nekretnina.Id).TipId = Convert.ToInt32(nekretnina.Tip);
            _context.Nekretnine.FirstOrDefault(nk => nk.Id == nekretnina.Id).NacinIznajmljivanjaId = Convert.ToInt32(nekretnina.NacinIznajmljivanja);

            _context.SaveChanges();
        }

        public IEnumerable<Nekretnina> GetNekretnine(int pagenumber, int pagesize)
        {
            if (pagenumber == 0)
                pagenumber = 1;
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
            if (pModel.SviTipovi != null)
            {
                foreach (var item in pModel.SviTipovi)
                {
                    if (item.Selected)
                        tipoviCheckedIds.Add(item.Id);
                }
            }
          
            List<int> nacinIznIds = new List<int>();
            if (pModel.NaciniIznajmljivanja != null)
            {
                foreach (var item in pModel.NaciniIznajmljivanja)
                {
                    if (item.Selected)
                        nacinIznIds.Add(item.Id);
                }
            }           
            IEnumerable<Nekretnina> nekretnine;

            if (pModel.Filter.CijenaMax == 0 && pModel.Filter.PovrsinaMax == 0 && pModel.Filter.DostupnoOd == DateTime.MinValue)
            {
                nekretnine = _context.Nekretnine.AsNoTracking().Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Where(n => n.Naslov.ToLower().Contains(pModel.SearchString.ToLower()) || n.Lokacija.ToLower().Contains(pModel.SearchString.ToLower()))
                                    .Include(k => k.Korisnik)
                                    .Include(m => m.Marker)
                                    .Include(t => t.Tip)
                                    .Include(ni => ni.NacinIznajmljivanja)
                                    .Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
            }
            else if (pModel.Filter.CijenaMax == 0 && pModel.Filter.PovrsinaMax == 0)
            {
                nekretnine = _context.Nekretnine.AsNoTracking().Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoOd) >= 0 && DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoDo) <= 0 && DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Where(n => n.Naslov.ToLower().Contains(pModel.SearchString.ToLower()) || n.Lokacija.ToLower().Contains(pModel.SearchString.ToLower()))
                                    .Include(k => k.Korisnik)
                                   .Include(m => m.Marker)
                                   .Include(t => t.Tip)
                                   .Include(ni => ni.NacinIznajmljivanja)
                                   .Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
            }
            else if (pModel.Filter.CijenaMax == 0 && pModel.Filter.DostupnoOd == DateTime.MinValue)
            {
                nekretnine = _context.Nekretnine.AsNoTracking().Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin && n.Povrsina <= pModel.Filter.PovrsinaMax)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Where(n => n.Naslov.ToLower().Contains(pModel.SearchString.ToLower()) || n.Lokacija.ToLower().Contains(pModel.SearchString.ToLower()))
                                    .Include(k => k.Korisnik)
                                    .Include(m => m.Marker)
                                    .Include(t => t.Tip)
                                    .Include(ni => ni.NacinIznajmljivanja)
                                    .Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
            }
            else if (pModel.Filter.PovrsinaMax == 0 && pModel.Filter.DostupnoOd == DateTime.MinValue)
            {
                nekretnine = _context.Nekretnine.AsNoTracking().Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin && n.Cijena <= pModel.Filter.CijenaMax)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Where(n => n.Naslov.ToLower().Contains(pModel.SearchString.ToLower()) || n.Lokacija.ToLower().Contains(pModel.SearchString.ToLower()))
                                    .Include(k => k.Korisnik)
                                    .Include(m => m.Marker)
                                    .Include(t => t.Tip)
                                    .Include(ni => ni.NacinIznajmljivanja)
                                    .Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
            }
            else if (pModel.Filter.CijenaMax == 0)
            {
                nekretnine = _context.Nekretnine.AsNoTracking().Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin && n.Povrsina <= pModel.Filter.PovrsinaMax)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoOd) >= 0 && DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoDo) <= 0 && DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Where(n => n.Naslov.ToLower().Contains(pModel.SearchString.ToLower()) || n.Lokacija.ToLower().Contains(pModel.SearchString.ToLower()))
                                    .Include(k => k.Korisnik)
                                    .Include(m => m.Marker)
                                    .Include(t => t.Tip)
                                    .Include(ni => ni.NacinIznajmljivanja)
                                    .Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
            }
            else if (pModel.Filter.PovrsinaMax == 0)
            {
                nekretnine = _context.Nekretnine.AsNoTracking().Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin && n.Cijena <= pModel.Filter.CijenaMax)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoOd) >= 0 && DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoDo) <= 0 && DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Where(n => n.Naslov.ToLower().Contains(pModel.SearchString.ToLower()) || n.Lokacija.ToLower().Contains(pModel.SearchString.ToLower()))
                                    .Include(k => k.Korisnik)
                                    .Include(m => m.Marker)
                                    .Include(t => t.Tip)
                                    .Include(ni => ni.NacinIznajmljivanja)
                                    .Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
            }
            else if (pModel.Filter.DostupnoOd == DateTime.MinValue)
            {
                nekretnine = _context.Nekretnine.AsNoTracking().Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin && n.Povrsina <= pModel.Filter.PovrsinaMax)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin && n.Cijena <= pModel.Filter.CijenaMax)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Where(n => n.Naslov.ToLower().Contains(pModel.SearchString.ToLower()) || n.Lokacija.ToLower().Contains(pModel.SearchString.ToLower()))
                                    .Include(k => k.Korisnik)
                                    .Include(m => m.Marker)
                                    .Include(t => t.Tip)
                                    .Include(ni => ni.NacinIznajmljivanja)
                                    .Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
            }
            else
            {
                nekretnine = _context.Nekretnine.AsNoTracking().Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin && n.Povrsina <= pModel.Filter.PovrsinaMax)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin && n.Cijena <= pModel.Filter.CijenaMax)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoOd) >= 0 && DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoDo) <= 0 && DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Where(n => n.Naslov.ToLower().Contains(pModel.SearchString.ToLower()) || n.Lokacija.ToLower().Contains(pModel.SearchString.ToLower()))
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
            if (pModel.SviTipovi != null)
            {
                foreach (var item in pModel.SviTipovi)
                {
                    if (item.Selected)
                        tipoviCheckedIds.Add(item.Id);
                }
            }
            
            List<int> nacinIznIds = new List<int>();
            if(pModel.NaciniIznajmljivanja != null)
            {
                foreach (var item in pModel.NaciniIznajmljivanja)
                {
                    if (item.Selected)
                        nacinIznIds.Add(item.Id);
                }
            }
            
            if (pModel.Filter.CijenaMax == 0 && pModel.Filter.PovrsinaMax == 0 && pModel.Filter.DostupnoOd == DateTime.MinValue)
            {
                return _context.Nekretnine.AsNoTracking().Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Where(n => n.Naslov.ToLower().Contains(pModel.SearchString.ToLower()) || n.Lokacija.ToLower().Contains(pModel.SearchString.ToLower()))
                                    .Count();
            }
            else if (pModel.Filter.CijenaMax == 0 && pModel.Filter.PovrsinaMax == 0)
            {
                return _context.Nekretnine.AsNoTracking().Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoOd) >= 0 && DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoDo) <= 0 && DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Where(n => n.Naslov.ToLower().Contains(pModel.SearchString.ToLower()) || n.Lokacija.ToLower().Contains(pModel.SearchString.ToLower()))
                                    .Count();
            }
            else if (pModel.Filter.CijenaMax == 0 && pModel.Filter.DostupnoOd == DateTime.MinValue)
            {
                return _context.Nekretnine.AsNoTracking().Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin && n.Povrsina <= pModel.Filter.PovrsinaMax)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Where(n => n.Naslov.ToLower().Contains(pModel.SearchString.ToLower()) || n.Lokacija.ToLower().Contains(pModel.SearchString.ToLower()))
                                    .Count();
            }
            else if (pModel.Filter.PovrsinaMax == 0 && pModel.Filter.DostupnoOd == DateTime.MinValue)
            {
                return _context.Nekretnine.AsNoTracking().Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin && n.Cijena <= pModel.Filter.CijenaMax)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Where(n => n.Naslov.ToLower().Contains(pModel.SearchString.ToLower()) || n.Lokacija.ToLower().Contains(pModel.SearchString.ToLower()))
                                    .Count();
            }
            else if (pModel.Filter.CijenaMax == 0)
            {
                return _context.Nekretnine.AsNoTracking().Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin && n.Povrsina <= pModel.Filter.PovrsinaMax)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoOd) >= 0 && DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoDo) <= 0 && DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Where(n => n.Naslov.ToLower().Contains(pModel.SearchString.ToLower()) || n.Lokacija.ToLower().Contains(pModel.SearchString.ToLower()))
                                    .Count();
            }
            else if (pModel.Filter.PovrsinaMax == 0)
            {
                return _context.Nekretnine.AsNoTracking().Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin && n.Cijena <= pModel.Filter.CijenaMax)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoOd) >= 0 && DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoDo) <= 0 && DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Where(n => n.Naslov.ToLower().Contains(pModel.SearchString.ToLower()) || n.Lokacija.ToLower().Contains(pModel.SearchString.ToLower()))
                                    .Count();
            }
            else if (pModel.Filter.DostupnoOd == DateTime.MinValue)
            {
                return _context.Nekretnine.AsNoTracking().Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin && n.Povrsina <= pModel.Filter.PovrsinaMax)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin && n.Cijena <= pModel.Filter.CijenaMax)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Where(n => n.Naslov.ToLower().Contains(pModel.SearchString.ToLower()) || n.Lokacija.ToLower().Contains(pModel.SearchString.ToLower()))
                                    .Count();
            }
            else
            {
                return _context.Nekretnine.AsNoTracking().Where(n => n.Povrsina >= pModel.Filter.PovrsinaMin && n.Povrsina <= pModel.Filter.PovrsinaMax)
                                    .Where(n => n.Cijena >= pModel.Filter.CijenaMin && n.Cijena <= pModel.Filter.CijenaMax)
                                    .Where(n => DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoOd) >= 0 && DateTime.Compare(pModel.Filter.DostupnoOd, n.DostupnoDo) <= 0 && DateTime.Compare(pModel.Filter.DostupnoDo, n.DostupnoDo) <= 0)
                                    .Where(n => tipoviCheckedIds.Contains(n.TipId))
                                    .Where(n => nacinIznIds.Contains(n.NacinIznajmljivanjaId))
                                    .Where(n => n.Naslov.ToLower().Contains(pModel.SearchString.ToLower()) || n.Lokacija.ToLower().Contains(pModel.SearchString.ToLower()))
                                    .Count();
            }
        }

        public IEnumerable<Tip> GetAllTipoviFiltera()
        {
            return _context.Tipovi;
        }

        public IEnumerable<NacinIznajmljivanja> GetAllNaciniIznajmljivanja()
        {
            return _context.NacinIznajmljivnja;
        }


        public void SetNewCoverImg(NekretninaImg img)
        {

            if (_context.NekretninaImgs.Where(ni => ni.IsCoverImg == true && ni.NekretninaId == img.NekretninaId).FirstOrDefault() != null)
                _context.NekretninaImgs.Where(ni => ni.IsCoverImg == true && ni.NekretninaId == img.NekretninaId).FirstOrDefault().IsCoverImg = false;
            _context.NekretninaImgs.Where(ni => ni.Id == img.Id).FirstOrDefault().IsCoverImg = true;
            _context.SaveChanges();
        }

        public async Task DeleteImgAsync(NekretninaImg img)
        {
            _context.NekretninaImgs.Attach(img);
            _context.NekretninaImgs.Remove(img);
            _context.SaveChanges();
            var publicIdList = new List<String>() { img.PublicId };
            await DeleteImgFromCloudinary(publicIdList);
        }

        public List<Komentar> GetKomentariForNekretnina(int nekretninaId)
        {
            return _context.Komentari
                .Include(k => k.Korisnik)
                .Where(k => k.NekretninaId == nekretninaId)
                .OrderByDescending(k => k.dateTime)
                .ToList();
        }

        public void AddKomentar(Komentar komentar)
        {
            _context.Komentari.Add(komentar);
            _context.SaveChanges();
        }

        private async Task DeleteImgFromCloudinary(List<string> publicIdsForDelete)
        {
            Account account = new Account(
                  "dysckfx4z",
                  "146882857231366",
                  "dOyF8Ue2EPJuNh73agNVjzKsxNk");

            Cloudinary cloudinary = new Cloudinary(account);

            var delParams = new DelResParams()
            {
                PublicIds = publicIdsForDelete,
                Invalidate = true
            };
            var delResult = await cloudinary.DeleteResourcesAsync(delParams);
        }


        private String GetNewImgName(int nekretninaId)
        {
            var imgs = _context.NekretninaImgs.Where(ni => ni.NekretninaId == nekretninaId).ToList();
            var last = new List<int>();
            foreach (var item in imgs)
            {
                var str = item.Url.Split('/').Last();
                str = str.Substring(0, str.LastIndexOf("."));
                last.Add(Convert.ToInt32(str));
            }

            return last.Count() == 0 ? 0.ToString() : (last.Max() + 1).ToString();
        }

        public void DeleteKomentar(int id)
        {
            var komentar = new Komentar { Id = id };
            _context.Komentari.Attach(komentar);
            _context.Komentari.Remove(komentar);
            _context.SaveChanges();
        }

        public void DeleteNekretnina(int id)
        {
            Nekretnina nekretnina = _context.Nekretnine.FirstOrDefault(nk => nk.Id == id);
            _context.Nekretnine.Remove(nekretnina);

            _context.SaveChanges();

        }

        public async Task DeleteNekretninaAsync(int id)
        {
            if (_context.NekretninaImgs.Where(n => n.Id == id).ToList() != null)
            {
                var imgsList = _context.NekretninaImgs.Where(n => n.NekretninaId == id).ToList();
                foreach (var item in imgsList)
                {
                    _context.NekretninaImgs.Attach(item);
                    _context.NekretninaImgs.Remove(item);
                    //_context.SaveChanges();
                    var publicIdList = new List<String>() { item.PublicId };
                    await DeleteImgFromCloudinary(publicIdList);
                }
            }
            if (_context.Komentari.Where(k => k.NekretninaId == id).ToList() != null)
            {
                var komList = _context.Komentari.Where(k => k.NekretninaId == id).ToList();
                foreach (var item in komList)
                {
                    _context.Komentari.Attach(item);
                    _context.Komentari.Remove(item);
                    //_context.SaveChanges();
                    //DeleteKomentar(item.Id);
                }
            }

            if (_context.Markeri.Where(m => m.Id == _context.Nekretnine.Where(n => n.Id == id).FirstOrDefault().MarkerId) != null)
            {
                var marker = new Marker { Id = _context.Nekretnine.Where(n => n.Id == id).FirstOrDefault().MarkerId };
                _context.Markeri.Attach(marker);
                _context.Markeri.Remove(marker);
            }

            var nekretnina = _context.ChangeTracker.Entries<Nekretnina>().FirstOrDefault(e => e.Entity.Id == id).Entity;

            _context.Nekretnine.Remove(nekretnina);
            _context.SaveChanges();
        }

        public int CountKomentari(int nekretninaid)
        {
            return _context.Komentari.Where(k => k.NekretninaId == nekretninaid).Count();
        }
        public MapViewModel GetNekretninasFiltered(MapViewModel model)
        {
            //Tipovi
            bool tipSelected = false;
            List<int> selectedTipIds = new List<int>();
            foreach (TipModel tip in model.SviTipovi)
            {
                if (tip.Selected == true)
                {
                    tipSelected = true;
                    selectedTipIds.Add(tip.Id);
                }
            }
            if (!tipSelected)
                foreach (TipModel tip in model.SviTipovi)
                {
                    tip.Selected = true;
                    selectedTipIds.Add(tip.Id);
                }

            //Nacini iznajmljivanja
            bool nacinSelected = false;
            List<int> selectedNacinIds = new List<int>();
            foreach (NacinIznajmljivanjaModel nacin in model.NaciniIznajmljivanja)
            {
                if (nacin.Selected == true)
                {
                    nacinSelected = true;
                    selectedNacinIds.Add(nacin.Id);
                }
            }
            if (!nacinSelected)
                foreach (NacinIznajmljivanjaModel nacin in model.NaciniIznajmljivanja)
                {
                    nacin.Selected = true;
                    selectedNacinIds.Add(nacin.Id);
                }

            //Dohvatanje iz baze i filtriranje po tipu i nacinu iznajmljivanja
            List<NekretninaListModel> nekretnine = _context.Nekretnine
                .Where(nek => selectedTipIds.Contains(nek.Tip.Id))
                .Where(nek => selectedNacinIds.Contains(nek.NacinIznajmljivanja.Id))
                .Select(nek => new NekretninaListModel
                {
                    Id = nek.Id,
                    Naslov = nek.Naslov,
                    Latitude = nek.Marker.Lat,
                    Longitude = nek.Marker.Lng,
                    NacinIznajmljivanja = nek.NacinIznajmljivanja.Naziv,
                    Tip = nek.Tip.ImeTipa,
                    Cijena = nek.Cijena,
                    Lokacija = nek.Lokacija,
                    Povrsina = nek.Povrsina,
                    DostupnoOd=nek.DostupnoOd,
                    DostupnoDo=nek.DostupnoDo
                })
                .ToList();

            //Filtriranje preko search stringa
            List<NekretninaListModel> nekretnineFiltered = new List<NekretninaListModel>();
            if (model.SearchString != null)
            {
                Regex regex = new Regex("[a-z0-9]{2,}");
                MatchCollection matchCollection = regex.Matches(model.SearchString.ToLower());
                foreach (NekretninaListModel nek in nekretnine)
                {
                    bool found = false;
                    foreach (Match match in matchCollection)
                    {
                        if (nek.Naslov.ToLower().Contains(match.Value) || nek.Lokacija.ToLower().Contains(match.Value))
                            found = true;
                    }
                    if (found)
                        nekretnineFiltered.Add(nek);
                }
                nekretnine = nekretnineFiltered;
            }

            //Filtriranje po cijeni, povrsini, dostupnosti
            if (model.Filter != null)
            {
                //Po cijeni
                if (model.Filter.CijenaMax > 0 && model.Filter.CijenaMax >= model.Filter.CijenaMin)
                    nekretnine = nekretnine.Where(nek => nek.Cijena >= model.Filter.CijenaMin && nek.Cijena <= model.Filter.CijenaMax).ToList();
                //Po povrsini
                if (model.Filter.PovrsinaMax > 0 && model.Filter.PovrsinaMax >= model.Filter.PovrsinaMin)
                    nekretnine = nekretnine.Where(nek => nek.Povrsina >= model.Filter.PovrsinaMin && nek.Povrsina <= model.Filter.PovrsinaMax).ToList();
                //Po dostupnosti
                if (model.Filter.DostupnoOd > DateTime.MinValue)
                    nekretnine = nekretnine.Where(nek => nek.DostupnoOd <= model.Filter.DostupnoOd).ToList();
                if (model.Filter.DostupnoDo > DateTime.MinValue)
                    nekretnine = nekretnine.Where(nek => nek.DostupnoDo >= model.Filter.DostupnoDo).ToList();
            }

            model.Nekretnine = nekretnine;
            return model;
        }

        public int CountNekretnineWithSearch(IEnumerable<Nekretnina> nekretninas, string searchString)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Nekretnina> SearchNekretnine(IEnumerable<Nekretnina> nekretninas, string searchString)
        {
            throw new NotImplementedException();
        }

        public string GetUserIdFromNekretnina(int nekretninaId)
        {
            return _context.Nekretnine.Include(k => k.Korisnik).FirstOrDefault(n => n.Id == nekretninaId).Korisnik.UserId;
        }


        private SelectList GettTipoviSelectList(int selected = 0)
        {
            return new SelectList(GetAllTipoviFiltera(), "Id", "ImeTipa", selected);
        }
        private SelectList GettNaciniIznajmljivanjaSelectList(int selected = 0)
        {
            return new SelectList(GetAllNaciniIznajmljivanja(), "Id", "Naziv", selected);
        }
    }
}
