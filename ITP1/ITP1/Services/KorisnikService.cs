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


        public Double GetProsjecnaOcjena(int jedinice, int dvice, int trice, int cetvorke, int petice)
        {
            return (jedinice + dvice + trice + cetvorke + petice) / 5;
        }
    }
}
