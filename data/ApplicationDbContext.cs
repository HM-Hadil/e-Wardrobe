﻿using e_commercedotNet.Models;
using Microsoft.EntityFrameworkCore;

namespace e_commercedotNet.data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Déclarez vos tables ici
        // Déclarez vos tables ici
        public DbSet<User> Users { get; set; } // Table des utilisateurs
        public DbSet<Product> Products { get; set; } // Table des produits
        public DbSet<CartItem> CartItems { get; set; }


    }
}