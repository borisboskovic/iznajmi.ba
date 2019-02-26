using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITP1.Data;
using ITP1.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ITP1.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private IKorisnik _korisnik;


        public ConfirmEmailModel(UserManager<IdentityUser> userManager, IKorisnik korisnik)
        {
            _userManager = userManager;
            _korisnik = korisnik;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Error confirming email for user with ID '{userId}':");
            }

            //Dodavanje i u tabelu Korisnik
            var noviKorisnik = new Korisnik()
            {
                UserId = userId,
                EMailFromAuthentication = user.UserName,
                Ime = user.PhoneNumber//Tu  je username :P
            };

            _korisnik.AddKorisnik(noviKorisnik);

            return Page();
        }
    }
}
