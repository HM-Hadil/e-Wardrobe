using System.Collections.Generic;

namespace e_commercedotNet.Models
{
    public class ConfirmationViewModel
    {
        public int UserId { get; set; }
        public List<CartItem> CartItems { get; set; }
        public decimal Total { get; set; }
    }
}
