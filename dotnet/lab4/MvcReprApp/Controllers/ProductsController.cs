using Microsoft.AspNetCore.Mvc;
using MvcReprApp.Models;
using System.Collections.Generic;
using System.Linq;


namespace MvcReprApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateProductController : ControllerBase
    {
        private static readonly List<Product> _products =
        [
            new() { Id = 1, Name = "Яблоко", Price = 1.20m },
            new() { Id = 2, Name = "Банан", Price = 0.80m }
        ];

        [HttpGet]
        public ActionResult<IEnumerable<CreateProductResponse>> GetAll()
        {
            return _products.ConvertAll(
                p => new CreateProductResponse { StatusCode = 0, Product = p }
            );
        }

        [HttpGet("{id}")]
        public ActionResult<CreateProductResponse> GetById(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            return new CreateProductResponse { StatusCode = 0, Product = product };
        }

        [HttpPost]
        public ActionResult<CreateProductResponse> Create(CreateProductRequest request)
        {
            Product product = new()
            {
                Id = _products.Max(p => p.Id) + 1,
                Name = request.Name,
                Price = request.Price
            };
            _products.Add(product);
            return new CreateProductResponse { StatusCode = 0, Product = product };
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CreateProductRequest request)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            product.Name = request.Name;
            product.Price = request.Price;
            return NoContent();
        }

        [HttpDelete]
        public IActionResult Delete(CreateProductRequest request)
        {
            var product = _products.FirstOrDefault(
                p => p.Name == request.Name && p.Price == request.Price
            );
            if (product == null) return NotFound();
            _products.Remove(product);
            return NoContent();
        }

        [HttpDelete("id")]
        public IActionResult DeleteById(int Id)
        {
            var product = _products.FirstOrDefault(p => p.Id == Id);
            if (product == null) return NotFound();
            _products.Remove(product);
            return NoContent();
        }
    }
}