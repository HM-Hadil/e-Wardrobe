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
        public DbSet<User> Users { get; set; }
    }
}