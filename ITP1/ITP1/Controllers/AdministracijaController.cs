using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ITP1.Data;
using ITP1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITP1.Controllers
{
    public class AdministracijaController : Controller
    {
        private readonly IAdministracija _administracija;
        private readonly UserManager<IdentityUser> _userManager;

        public AdministracijaController(IAdministracija administracija, UserManager<IdentityUser> userManager)
        {
            _administracija = administracija;
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [Authorize]
        public async Task<IActionResult> Index(AdministracijaModel adminModel)
        {
            if (await IsCurrentUserInRoleAsync("Admin"))
            {
                adminModel.SearchString = adminModel.SearchString == null ? "" : adminModel.SearchString;
                adminModel.CurrPage = adminModel.CurrPage == 0 ? 1 : adminModel.CurrPage;
                adminModel.Pager = new Pager(_administracija.CountKoriscnici(adminModel.SearchString), adminModel.CurrPage);
                var korisnici = _administracija.GetKorisnici(adminModel.Pager.CurrentPage, adminModel.Pager.PageSize, adminModel.SearchString);

                adminModel.Korisnici = new List<KorisnikAdministracijaModel>();
                foreach (var item in korisnici)
                {
                    adminModel.Korisnici.Add(
                        new KorisnikAdministracijaModel()
                        {
                            Id = item.Id,
                            UserId = item.UserId,
                            Ime = item.Ime,
                            Email = item.EMailFromAuthentication,
                            Role = _administracija.GetUserRole(item.UserId),
                        }
                    );
                }

                return View(adminModel);
                
            }

            //Todo error page
            return View();
        }

        [Authorize]
        public async Task<IActionResult> DeleteUser(string userId, int korisnikId)
        {
            if (await IsCurrentUserInRoleAsync("Admin"))
            {
                await _administracija.DeleteUserAsync(userId, korisnikId);
                return RedirectToAction("Index", "Administracija");
            }

            //todoooooo
            return null;
        }

        [Authorize]
        public async Task<IActionResult> GiveAdminRoleToUser(string userId)
        {
            if (await IsCurrentUserInRoleAsync("Admin"))
            {
                await _administracija.GiveUserRoleAsync(userId, "Admin");
                return RedirectToAction("Index", "Administracija");
            }

            //todoooooo
            return null;
        }


        private async Task<bool> IsCurrentUserInRoleAsync(string role)
        {
            var user = await _userManager.FindByIdAsync(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
                return true;

            return false;
        }
    }
}