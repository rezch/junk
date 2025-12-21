namespace lab15.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class CreateProductRequest
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class CreateProductResponse
    {
        public int StatusCode { get; set; }
        public Product Product { get; set; }
    }
}