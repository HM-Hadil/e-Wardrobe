namespace e_commercedotNet.Models
{
    public class ProductListViewModel
    {
        public List<Product> Products { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
