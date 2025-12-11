using lab3.Models;

namespace lab3.Services
{
    public class ProductService
    {
        private readonly List<Product> _products = [];
        private int _nextId = 1;

        public List<Product> GetAll()
        {
            return _products;
        }

        public Product? GetById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public Product Create(Product product)
        {
            product.Id = _nextId++;
            _products.Add(product);
            return product;
        }

        public bool Update(int id, Product updatedProduct)
        {
            var product = GetById(id);
            if (product == null) return false;

            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;

            return true;
        }

        public bool Delete(int id)
        {
            var product = GetById(id);
            if (product == null) return false;
            return _products.Remove(product);
        }
    }
}