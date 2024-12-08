using e_commercedotNet.data;
using e_commercedotNet.Models;
using Microsoft.AspNetCore.Mvc;

namespace e_commercedotNet.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructeur unique prenant ApplicationDbContext
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
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

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // Nettoyer les entrées pour éviter des erreurs dues à des espaces invisibles
            email = email?.Trim().ToLower();
            password = password?.Trim();

            // Recherche de l'utilisateur dans la base de données
            var user = _context.Users.FirstOrDefault(u =>
                u.Email.Trim().ToLower() == email &&
                u.Password.Trim() == password);

            if (user != null)
            {
                // Connexion réussie
                TempData["SuccessMessage"] = $"Bienvenue, {email}!";
                return RedirectToAction("Index", "Home");
            }

            // Connexion échouée
            TempData["ErrorMessage"] = "Email ou mot de passe incorrect. Veuillez réessayer.";
            return View();
        }

    }
}
