using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ITP1.Data;
using ITP1.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITP1.Models;
using Microsoft.EntityFrameworkCore;

namespace ITP1.Services
{
    public class KorisnikService : IKorisnik
    {
        private ApplicationDbContext _context;
        public KorisnikService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int GetIdWithForeignKey(string userId)
        {
            return _context.Korisnici.FirstOrDefault(k => k.UserId == userId).Id;
        }

        public void AddKorisnik(Korisnik newKorisnik)
        {
            _context.Korisnici.Add(newKorisnik);
            _context.SaveChanges();
        }

        public void AddAspNetUser(IdentityUser user)//Unecessary
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateKorisnik(Korisnik korisnik)
        {
            _context.Korisnici.FirstOrDefault(k => k.Id == korisnik.Id).Ime = korisnik.Ime;
            _context.Korisnici.FirstOrDefault(k => k.Id == korisnik.Id).MailKontakt = korisnik.MailKontakt;
            _context.Korisnici.FirstOrDefault(k => k.Id == korisnik.Id).Tel = korisnik.Tel;
            _context.Korisnici.FirstOrDefault(k => k.Id == korisnik.Id).WebKontaktUrl = korisnik.WebKontaktUrl;
            _context.Korisnici.FirstOrDefault(k => k.Id == korisnik.Id).AvatarImgUrl = korisnik.AvatarImgUrl;
            _context.SaveChanges();
        }


        public IEnumerable<Korisnik> GetAll()
        {
            return _context.Korisnici;
        }

        public Korisnik GetKorisnik(int id)
        {
            return _context.Korisnici
                .FirstOrDefault(k => k.Id == id);
        }

        public string GetAvatarImgUrl(int id)
        {
            return _context.Korisnici
                .FirstOrDefault(k => k.Id == id).AvatarImgUrl;
        }

        public string GetForeignKeyAspNetUsersId(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email).Id;
        }

        public Korisnik GetKorisnik(string email)
        {
            return _context.Korisnici.FirstOrDefault(k => k.EMailFromAuthentication == email);
        }

        public Korisnik GetKorisnikWithForeignKey(string id)
        {
            return _context.Korisnici.FirstOrDefault(k => k.UserId == id);
        }


        public string GetUsernameFromAspNetUsers(string id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id).UserName;//UserName in AspNetUsers table is email.
        }

        public Utisak GetUtisak(int korisnikId, int ocijenjeniKorisnikId)
        {
            return _context.Utisci
                .FirstOrDefault(u => u.KorisnikId == korisnikId && u.OcjenjeniKorinsnikid == ocijenjeniKorisnikId)
;
        }

        //Todo možda neće trebati logged user ovdje
        public List<Utisak> GetUtisci(int ocijenjeniKorisnikId, int loggedUser)
        {
            //return _context.Utisci.Where(u => u.OcjenjeniKorinsnikid == ocijenjeniKorisnikId && u.KorisnikId != loggedUser)
            //    .Include(u => u.Korisnik)
            //    .ToList();
            return _context.Utisci.Where(u => u.OcjenjeniKorinsnikid == ocijenjeniKorisnikId)
                .Include(u => u.Korisnik)
                .ToList();
        }

        public void AddUtisak(Utisak utisak)
        {
            if (_context.Utisci.Where(u => u.KorisnikId == utisak.KorisnikId && u.OcjenjeniKorinsnikid == utisak.OcjenjeniKorinsnikid).Count() > 0)
            {
                _context.Utisci.Where(u => u.KorisnikId == utisak.KorisnikId && u.OcjenjeniKorinsnikid == utisak.OcjenjeniKorinsnikid).FirstOrDefault().Ocjena = utisak.Ocjena;
                _context.Utisci.Where(u => u.KorisnikId == utisak.KorisnikId && u.OcjenjeniKorinsnikid == utisak.OcjenjeniKorinsnikid).FirstOrDefault().Komentar = utisak.Komentar;
            }
            else
            {
                _context.Utisci.Add(utisak);
            }
            _context.SaveChanges();
        }


        public async Task UpdateUserImgToCloudAsync(string path, string imgName, string extension)
        {
            CloudinaryDotNet.Account account = new CloudinaryDotNet.Account(
                                      "dysckfx4z",
                                      "146882857231366",
                                      "dOyF8Ue2EPJuNh73agNVjzKsxNk");

            Cloudinary cloudinary = new Cloudinary(account);
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(@path),
                PublicId = @"Users/" + imgName + extension,
            };
            var uploadResult = await cloudinary.UploadAsync(uploadParams);
            ChangeAvatarImgUrl(uploadResult.Uri.ToString(), imgName);
        }

        //imgName is users primary key and Korisnici foreign key
        public async Task<string> UpdateUserImgToCloudWithStreamAsync(IFormFile formFile, string imgName, int korisnikId)
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
                        PublicId = @"Users/" + imgName,
                        Transformation = new Transformation().Width(400).Height(400).Crop("limit"),
                    };
                    var uploadResult = await cloudinary.UploadAsync(uploadParams);
                    return uploadResult.Uri.ToString();
                }
            }
            return "";
        }


        private void ChangeAvatarImgUrl(string url, string userId)
        {
            _context.Korisnici.FirstOrDefault(k => k.UserId == userId).AvatarImgUrl = url;
            _context.SaveChanges();
        }

        public Double GetProsjecnaOcjena(int korisnikId)
        {
            return _context.Utisci.Where(u => u.OcjenjeniKorinsnikid == korisnikId).Count() > 0 ?
                _context.Utisci
                .Where(u => u.OcjenjeniKorinsnikid == korisnikId)
                .Average(u => u.Ocjena)
                :
                0;
        }

        public int GetTotalNumberOcjena(int korisnikId)
        {
            return _context.Utisci
                .Where(u => u.OcjenjeniKorinsnikid == korisnikId)
                .Count();
        }


        public List<NekretninaItem> GetListaNekretninaZaKorisnika(int korisnikId)
        {
            IEnumerable<Nekretnina> nekretnine = _context.Nekretnine.Where(n => n.KorisnikId == korisnikId).Include(t => t.Tip).Include(ni => ni.NacinIznajmljivanja).ToList();
            List<NekretninaItem> nekretnine_item = new List<NekretninaItem>();
            var slidze = _context.NekretninaImgs.ToList();

            foreach (var item in nekretnine)
            {
                NekretninaItem nekretnina_item = new NekretninaItem()
                {
                    Cijena = item.Cijena,
                    Id = item.Id,
                    Naslov = item.Naslov,
                    Korisik = item.Korisnik,
                    Lokacija = item.Lokacija,
                    Povrsina = item.Povrsina,
                    DostupnoOd = item.DostupnoOd,
                    DostupnoDo = item.DostupnoDo,
                    CoverImgUrl = _context.NekretninaImgs.Where(ni => ni.NekretninaId == item.Id && ni.IsCoverImg == true).FirstOrDefault() == null ? null : _context.NekretninaImgs.Where(ni => ni.NekretninaId == item.Id && ni.IsCoverImg == true).FirstOrDefault().Url,
                    Tip = new TipModel()
                    {
                        Id = item.Tip == null ? 0 : item.Tip.Id,
                        ImeTipa = item.Tip == null ? null : item.Tip.ImeTipa
                    },
                    NacinIznajmljivanja = new NacinIznajmljivanjaModel()
                    {
                        Id = item.NacinIznajmljivanja == null ? 0 : item.NacinIznajmljivanja.Id,
                        Naziv = item.NacinIznajmljivanja == null ? null : item.NacinIznajmljivanja.Naziv,
                    }
                    ,
                };
                nekretnine_item.Add(nekretnina_item);
            }

            return nekretnine_item;
        }
    }
}
