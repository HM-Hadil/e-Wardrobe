using e_commercedotNet.Models;
using Microsoft.AspNetCore.Mvc;

namespace e_commercedotNet.Controllers
{
    // Controllers/AccountController.cs
    public class AccountController : Controller
    {
        // Afficher le formulaire d'inscription
        public IActionResult Register()
        {
            // Assurez-vous d'initialiser le modèle avant de le passer à la vue
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }


    }
    
    
}