using e_commercedotNet.data;
using e_commercedotNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace e_commercedotNet.Controllers
{
    [Route("Product")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 10; // You can adjust this value for the number of items per page


        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Product/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            var product = new Product(); // Initialize the model
            return View("CreateProduct", product); // Pass the model to the view
        }

        // POST: Product/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product); // Add the product to the database
                _context.SaveChanges(); // Save changes to the database
                TempData["SuccessMessage"] = "Produit ajouté avec succès."; // Success message
                return RedirectToAction("ListProduct"); // Redirect to ListProduct after successful creation
            }
            return View("CreateProduct", product); // Return the view with validation errors
        }
        // GET: Product/ListProduct
        [HttpGet("ListProduct")] // This route should be unique
        public IActionResult ListProduct(int page = 1)
        {
            // Calculate total number of products
            var totalProducts = _context.Products.Count();

            // Calculate total pages based on the page size
            var totalPages = (int)Math.Ceiling((double)totalProducts / PageSize);

            // Fetch the products for the current page
            var products = _context.Products
                                    .Skip((page - 1) * PageSize) // Skip the products from previous pages
                                    .Take(PageSize)             // Take the products for the current page
                                    .ToList();

            // Create a view model to pass to the view
            var model = new ProductListViewModel
            {
                Products = products,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(model);
        }
        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product); // Ensure the correct product is passed
        }
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest("The product ID does not match the route ID.");
            }

            if (ModelState.IsValid)
            {
                var existingProduct = _context.Products.FirstOrDefault(p => p.ProductId == id);
                if (existingProduct == null)
                {
                    return NotFound("The product does not exist.");
                }

                // Update product properties
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.StockQuantity = product.StockQuantity;
                existingProduct.Category = product.Category;

                // Save changes to the database
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Produit modifié avec succès.";
                return RedirectToAction("ListProduct");
            }

            // If the model state is invalid, return the view with the current product data
            return View(product);
        }

        // GET: Product/Delete/{id}
        [HttpGet("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id); // Fetch the product by ID
            if (product == null)
            {
                return NotFound();
            }
            return View(product); // Display delete confirmation view
        }

        // POST: Product/Delete/{id}
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _context.Products.Find(id); // Fetch the product by ID
            if (product != null)
            {
                _context.Products.Remove(product); // Remove the product from the database
                _context.SaveChanges(); // Save changes to the database
                TempData["SuccessMessage"] = "Produit supprimé avec succès."; // Success message
            }
            return RedirectToAction(nameof(ListProduct)); // Redirect to the product list after deletion
        }


        // Voir les détails du produit
        public IActionResult Details(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Produit non trouvé.";
                return RedirectToAction("Index", "Home");
            }

            return View(product);  // Passe le produit à la vue
        }
        
        
    }
}
