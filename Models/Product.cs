using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commercedotNet.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [Precision(18, 2)] 
        public decimal Price { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        public string Description { get; set; }

        [Required]
        public string Category { get; set; }

        public string? ImageUrl { get; set; } = "";
       

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
