namespace e_commercedotNet.Models
{
    public class Product
    {
        public int ProductId { get; set; } // Identifiant unique du produit
        public string Name { get; set; } // Nom du produit
        public string Description { get; set; } // Description du produit
        public decimal Price { get; set; } // Prix du produit
        public int StockQuantity { get; set; } // Quantité en stock
        public string Category { get; set; } // Catégorie du produit ("Homme" ou "Femme")
        public string ImageUrl { get; set; } // URL de l'image du produit
    }
}
