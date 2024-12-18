using System.Diagnostics;
using e_commercedotNet.Models;
using Microsoft.AspNetCore.Mvc;
using e_commercedotNet.data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;  // Ajouté pour l'usage de ToListAsync()
using System.Linq;
using System.Threading.Tasks; // Pour la gestion asynchrone

namespace e_commercedotNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Utilisation de Task<IActionResult> pour gérer les opérations asynchrones
        public async Task<IActionResult> Index(string Category, decimal? PriceMin, decimal? PriceMax)
        {
            // Store filters in ViewData for easy access in the view
            ViewData["Category"] = Category;
            ViewData["PriceMin"] = PriceMin;
            ViewData["PriceMax"] = PriceMax;

            // Start with all products
            var products = _context.Products.AsQueryable();

            // Filter by category if provided
            if (!string.IsNullOrEmpty(Category))
            {
                products = products.Where(p => p.Category == Category);
            }

            // Filter by price range if provided
            if (PriceMin.HasValue)
            {
                products = products.Where(p => p.Price < PriceMin.Value);
            }
            if (PriceMax.HasValue)
            {
                products = products.Where(p => p.Price <= PriceMax.Value);
            }

            // Return the filtered list of products to the view, using async method
            var productList = await products.ToListAsync();
            return View(productList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Ajouter une méthode asynchrone pour obtenir les détails du produit
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
