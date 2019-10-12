using ITP1.Data.Models;
using ITP1.Models;
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

        //Foreign Key, username is email
        string GetUsernameFromAspNetUsers(string id);
        void AddAspNetUser(Microsoft.AspNetCore.Identity.IdentityUser user);
        string GetForeignKeyAspNetUsersId(string email);
        List<NekretninaItem> GetListaNekretninaZaKorisnika(int id);

        Utisak GetUtisak(int korisnikId, int ocijenjeniKorisnikId);
        List<Utisak> GetUtisci(int ocijenjeniKorisnikId, int loggedUser);
        void AddUtisak(Utisak utisak);
        int GetTotalNumberOcjena(int korisnikId);
        Double GetProsjecnaOcjena(int korisnikId);
    }
}
