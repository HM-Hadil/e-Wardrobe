using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace e_commercedotNet.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        public int UserId { get; set; } // Clé étrangère vers l'utilisateur
        public int ProductId { get; set; } // Clé étrangère vers le produit
        public int Quantity { get; set; }
        public User User { get; set; } // Navigation property
        public Product Product { get; set; } 
    }
    }
