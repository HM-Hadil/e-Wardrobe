﻿using e_commercedotNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using e_commercedotNet.data;

namespace e_commercedotNet.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public AccountController(
      ApplicationDbContext context,
      IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register")]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Votre compte a été créé avec succès ! Vous pouvez maintenant vous connecter.";
                return RedirectToAction("Login");
            }

            return View(user);
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Email ou mot de passe incorrect.";
                return View();
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Email),
        new Claim("FullName", $"{user.Prenom} {user.Nom}")
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            HttpContext.Session.SetString("UserId", user.id.ToString());

            return RedirectToAction("Profile", "Account");
        }


        [HttpGet("Profile")]
        public IActionResult Profile()
        {
            var email = User.Identity?.Name;

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login");
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost("Profile")]
        public IActionResult Profile(User updatedUser)
        {
            if (ModelState.IsValid)
            {
                var email = User.Identity?.Name;

                if (string.IsNullOrEmpty(email))
                {
                    TempData["ErrorMessage"] = "Vous devez être connecté pour mettre à jour votre profil.";
                    return RedirectToAction("Login");
                }

                var currentUser = _context.Users.FirstOrDefault(u => u.Email == email);

                if (currentUser != null)
                {
                    currentUser.Nom = updatedUser.Nom;
                    currentUser.Prenom = updatedUser.Prenom;
                    currentUser.Password = updatedUser.Password;

                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Votre profil a été mis à jour avec succès!";
                    return RedirectToAction("Profile");
                }
            }

            TempData["ErrorMessage"] = "Une erreur s'est produite lors de la mise à jour de votre profil.";
            return View(updatedUser);
        }

        [HttpPost("UpdateProfile")]
        public IActionResult UpdateProfile(User updatedUser)
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login");
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return NotFound();
            }

            user.Nom = updatedUser.Nom;
            user.Prenom = updatedUser.Prenom;
            user.Email = updatedUser.Email;

            if (!string.IsNullOrEmpty(updatedUser.Password))
            {
                user.Password = updatedUser.Password; 
            }

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Votre profil a été mis à jour avec succès!";
            return RedirectToAction("Profile");
        }
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }


    }


}
