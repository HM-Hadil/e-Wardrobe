using e_commercedotNet.Models;
using Microsoft.EntityFrameworkCore;

namespace e_commercedotNet.data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

      
        public DbSet<User> Users { get; set; } 
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }


    }
}