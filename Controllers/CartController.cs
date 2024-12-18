using Microsoft.AspNetCore.Mvc;
using e_commercedotNet.Models;
using e_commercedotNet.data;
using Microsoft.EntityFrameworkCore;

namespace e_commercedotNet.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost("AddToCart")]
        public IActionResult AddToCart(int productId, int quantity)
        {
            // Récupérer l'ID utilisateur de la session
            var userIdString = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdString))
            {
                TempData["ErrorMessage"] = "Vous devez être connecté pour ajouter un produit au panier.";
                return RedirectToAction("Login", "Account");
            }

            if (!int.TryParse(userIdString, out int userId))
            {
                TempData["ErrorMessage"] = "Erreur de connexion. Veuillez vous reconnecter.";
                return RedirectToAction("Login", "Account");
            }

            // Vérifier l'existence du produit
            var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Produit non trouvé.";
                return RedirectToAction("Index", "Home");
            }

            // Vérifier la disponibilité du stock
            if (product.StockQuantity < quantity)
            {
                TempData["ErrorMessage"] = "Stock insuffisant pour ce produit.";
                return RedirectToAction("Details", "Home", new { id = productId });
            }

            // Vérifier si le produit existe déjà dans le panier pour cet utilisateur
            var existingCartItem = _context.CartItems
                .FirstOrDefault(ci => ci.UserId == userId && ci.ProductId == productId);

            if (existingCartItem != null)
            {
                // Mettre à jour la quantité existante
                existingCartItem.Quantity += quantity;
            }
            else
            {
                // Créer un nouvel article dans le panier
                var cartItem = new CartItem
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                };
                _context.CartItems.Add(cartItem);
            }

            // Mettre à jour le stock du produit
            product.StockQuantity -= quantity;

            try
            {
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Produit ajouté au panier avec succès.";
            }
            catch (Exception ex)
            {
                // Gestion des erreurs
                Console.WriteLine($"Erreur lors de l'ajout au panier : {ex.Message}");
                TempData["ErrorMessage"] = "Une erreur est survenue lors de l'ajout au panier.";
            }

            return RedirectToAction("Index", "Cart");
        }
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            if (!int.TryParse(userId, out int parsedUserId))
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.UserId == parsedUserId)
                .ToListAsync();

            // Calculate the total quantity of items in the cart
            var totalQuantity = cartItems.Sum(ci => ci.Quantity);
            ViewBag.CartItemCount = totalQuantity; // Pass the count to the view

            var total = cartItems.Sum(ci => ci.Quantity * ci.Product.Price);
            ViewBag.Total = total;

            if (!cartItems.Any())
            {
                TempData["ErrorMessage"] = "Votre panier est vide.";
            }

            return View(cartItems);
        }
        // Update the cart (change quantity or remove item)
        [HttpPost]
        public async Task<IActionResult> UpdateCart(Dictionary<int, int> quantities, int? removeItemId)
        {
            var userId = HttpContext.Session.GetString("UserId");

            // Vérifier si l'utilisateur est connecté
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "Vous devez être connecté pour gérer votre panier.";
                return RedirectToAction("Login", "Account");
            }

            // Vérifier si l'ID de l'utilisateur est valide
            if (!int.TryParse(userId, out int parsedUserId))
            {
                TempData["ErrorMessage"] = "Erreur de connexion. Veuillez vous reconnecter.";
                return RedirectToAction("Login", "Account");
            }

            // Gestion de la suppression d'un article du panier
            if (removeItemId.HasValue)
            {
                var cartItem = await _context.CartItems
                    .FirstOrDefaultAsync(ci => ci.CartItemId == removeItemId.Value && ci.UserId == parsedUserId);

                if (cartItem != null)
                {
                    _context.CartItems.Remove(cartItem);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Produit supprimé du panier.";
                }
                else
                {
                    TempData["ErrorMessage"] = "L'article n'a pas été trouvé dans votre panier.";
                }
            }

            // Mise à jour des quantités des produits dans le panier
            foreach (var item in quantities)
            {
                var cartItem = await _context.CartItems
                    .FirstOrDefaultAsync(ci => ci.CartItemId == item.Key && ci.UserId == parsedUserId);

                if (cartItem != null)
                {
                    if (item.Value < 1) // Vérification que la quantité est valide
                    {
                        TempData["ErrorMessage"] = "La quantité doit être au moins de 1.";
                        return RedirectToAction("Index");
                    }

                    cartItem.Quantity = item.Value;
                }
                else
                {
                    TempData["ErrorMessage"] = $"L'article avec l'ID {item.Key} n'existe pas dans votre panier.";
                    return RedirectToAction("Index");
                }
            }

            // Sauvegarder les modifications dans la base de données
            await _context.SaveChangesAsync();

            // Calculer le nombre total d'articles dans le panier
            var cartItems = await _context.CartItems.Where(ci => ci.UserId == parsedUserId).ToListAsync();
            ViewBag.CartItemCount = cartItems.Sum(ci => ci.Quantity);

            TempData["SuccessMessage"] = "Votre panier a été mis à jour avec succès.";

            // Rediriger vers la page du panier avec les mises à jour
            return RedirectToAction("Index");
        }



        [HttpPost]
        [Route("ConfirmPurchase")]
        public async Task<IActionResult> ConfirmPurchase()
        {
            var userIdString = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdString))
            {
                TempData["ErrorMessage"] = "Vous devez être connecté pour confirmer l'achat.";
                return RedirectToAction("Login", "Account");
            }

            if (!int.TryParse(userIdString, out int userId))
            {
                TempData["ErrorMessage"] = "Erreur de connexion. Veuillez vous reconnecter.";
                return RedirectToAction("Login", "Account");
            }

            // Récupérer les articles du panier
            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                TempData["ErrorMessage"] = "Votre panier est vide.";
                return RedirectToAction("Index");
            }

            var total = cartItems.Sum(ci => ci.Quantity * ci.Product.Price);

            // Passer les informations à la vue de confirmation
            var confirmationViewModel = new ConfirmationViewModel
            {
                UserId = userId,
                CartItems = cartItems,
                Total = total
            };

            // Rediriger vers la page de confirmation
            return View("Confirmation", confirmationViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> FinalizePurchase()
        {
            var userIdString = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "Account");
            }

            if (!int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // Récupérer les articles du panier
            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                TempData["ErrorMessage"] = "Votre panier est vide.";
                return RedirectToAction("Index");
            }

            // Nettoyer le panier
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Votre commande a été confirmée.";

            // Redirection vers la page d'accueil
            return RedirectToAction("Index", "Home");
        }
    }





}
