using ITP1.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITP1.Data
{
    public interface IAdministracija
    {
        int CountKoriscnici(string searchString);
        List<Korisnik> GetKorisnici(int pagenumber, int pagesize, string searchString);
        String GetUserRole(string userId);
        Task GiveUserRoleAsync(string userId, string role);
        Task DeleteUserAsync(string userId, int korisnikId);
    }
}
