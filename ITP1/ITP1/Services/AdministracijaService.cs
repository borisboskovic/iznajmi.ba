using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ITP1.Data;
using ITP1.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITP1.Services
{
    public class AdministracijaService : IAdministracija
    {
        private ApplicationDbContext _context;
        public AdministracijaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int CountKoriscnici(string searchString)
        {
            return _context.Korisnici
                .Where(k => k.Ime.ToLower().Contains(searchString.ToLower()) || k.MailKontakt.ToLower().Contains(searchString.ToLower()))
                .Count();
        }

        public List<Korisnik> GetKorisnici(int pagenumber, int pagesize, string searchString)
        {
            return _context.Korisnici
                .Where(k => k.Ime.ToLower().Contains(searchString.ToLower()) || k.MailKontakt.ToLower().Contains(searchString.ToLower()))
                .Skip((pagenumber - 1) * pagesize).Take(pagesize).ToList();
        }

        public String GetUserRole(string userId)
        {
            if (_context.UserRoles.Where(ur => ur.UserId == userId).FirstOrDefault() != null)
            {
                var roleId = _context.UserRoles.Where(ur => ur.UserId == userId).FirstOrDefault().RoleId;
                return _context.Roles.Where(r => r.Id == roleId).FirstOrDefault().Name;
            }
            return "Korisnik";
        }

        public async Task GiveUserRoleAsync(string userId, string role)
        {
            string roleId = _context.Roles.Where(r => r.Name == role).FirstOrDefault().Id;
            String query = "Insert into AspNetUserRoles (UserId, RoleId) Values('" + userId + "', '" + roleId + "')";
#pragma warning disable EF1000 // Possible SQL injection vulnerability.
            _context.Database.ExecuteSqlCommand(query);
#pragma warning restore EF1000 // Possible SQL injection vulnerability.
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(string userId, int korisnikId)
        {
            var utisci = _context.Utisci.Where(ut => ut.KorisnikId == korisnikId || ut.OcjenjeniKorinsnikid == korisnikId).ToList();
            foreach (var item in utisci)
            {
                _context.Utisci.Attach(item);
                _context.Utisci.Remove(item);
            }

            var komentari = _context.Komentari.Where(kom => kom.KorisnikId == korisnikId).ToList();
            foreach (var item in komentari)
            {
                _context.Komentari.Attach(item);
                _context.Komentari.Remove(item);
            }

            var nekretnine = _context.Nekretnine.Where(n => n.KorisnikId == korisnikId).ToList();
            foreach (var item in nekretnine)
            {
                var imgsList = _context.NekretninaImgs.Where(n => n.NekretninaId == item.Id).ToList();
                foreach (var itemImg in imgsList)
                {
                    _context.NekretninaImgs.Attach(itemImg);
                    _context.NekretninaImgs.Remove(itemImg);
                    var publicIdList = new List<String>() { itemImg.PublicId };
                    await DeleteImgFromCloudinary(publicIdList);
                }

                var nekretninaKom = _context.Komentari.Where(kom => kom.NekretninaId == item.Id).ToList();
                foreach (var komItem in nekretninaKom)
                {
                    _context.Komentari.Attach(komItem);
                    _context.Komentari.Remove(komItem);
                }

                if (_context.Markeri.Where(m => m.Id == item.MarkerId).FirstOrDefault() != null)
                {
                    var marker = _context.Markeri.Where(m => m.Id == item.MarkerId).FirstOrDefault();
                    _context.Attach(marker);
                    _context.Remove(marker);
                }
            }

            if (_context.UserRoles.Where(ur => ur.UserId == userId).FirstOrDefault() != null)
            {
                var userRole = _context.UserRoles.Where(ur => ur.UserId == userId).FirstOrDefault();
                _context.UserRoles.Attach(userRole);
                _context.Remove(userRole);
            }

            var user = _context.Users.Where(u => u.Id == userId).FirstOrDefault();
            _context.Users.Attach(user);
            _context.Remove(user);

            var korisnik = _context.Korisnici.Where(k => k.Id == korisnikId).FirstOrDefault();
            _context.Korisnici.Attach(korisnik);
            _context.Korisnici.Remove(korisnik);

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

    }
}
