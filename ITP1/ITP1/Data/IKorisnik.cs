using ITP1.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITP1.Data
{
    public interface IKorisnik
    {
        void AddKorisnik(Korisnik newKorisnik);
        int GetIdWithForeignKey(string userId);
        Task UpdateUserImgToCloudAsync(string path, string imgName, string extension);
        Task<string> UpdateUserImgToCloudWithStreamAsync(IFormFile formFile, string imgName, int korisnikId);
        Korisnik GetKorisnik(int id);
        Korisnik GetKorisnik(string email);
        Korisnik GetKorisnikWithForeignKey(string id);
        string GetAvatarImgUrl(int id);
        void UpdateKorisnik(Korisnik korisnik);
        IEnumerable<Korisnik> GetAll();
        Double GetProsjecnaOcjena(int jedinice, int dvice, int trice, int cetvorke, int petice);

        //Foreign Key, username is email
        string GetUsernameFromAspNetUsers(string id);
        void AddAspNetUser(Microsoft.AspNetCore.Identity.IdentityUser user);
        string GetForeignKeyAspNetUsersId(string email);
    }
}
