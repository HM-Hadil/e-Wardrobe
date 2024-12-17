namespace e_commercedotNet.Models
{
    public class ProductListViewModel
    {
        public required List<Product> Products { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
