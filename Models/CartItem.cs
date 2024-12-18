using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace e_commercedotNet.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        public int UserId { get; set; } 
        public int ProductId { get; set; } 
        public int Quantity { get; set; }
        public User User { get; set; } 
        public Product Product { get; set; } 
    }
    }
