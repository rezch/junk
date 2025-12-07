using Microsoft.AspNetCore.Mvc;
using MvcReprApp.Models;


namespace MvcReprApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateProductController : ControllerBase
    {
        // TODO: to remove
        private static readonly List<Product> _products =
        [
            new() { Id = 1, Name = "Яблоко", Price = 1.20m },
            new() { Id = 2, Name = "Банан", Price = 0.80m }
        ];

        private readonly DatabaseHelper _dbHelper = new();

        [HttpGet]
        public ActionResult<IEnumerable<CreateProductResponse>> GetAll()
        {
            return _dbHelper.GetStudents();
        }

        [HttpGet("{id}")]
        public ActionResult<CreateProductResponse> GetById(int id)
        {
            // TODO
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            return new CreateProductResponse { StatusCode = 0, Product = product };
        }

        [HttpPost]
        public ActionResult<CreateProductResponse> Create(CreateProductRequest request)
        {
            return _dbHelper.AddProduct(request)
                ? Ok()
                : StatusCode(500, "При добавлении произошла ошибка");
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CreateProductRequest request)
        {
            // TODO
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            product.Name = request.Name;
            product.Price = request.Price;
            return NoContent();
        }

        [HttpDelete]
        public IActionResult Delete(CreateProductRequest request)
        {
            // TODO
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
            // TODO
            var product = _products.FirstOrDefault(p => p.Id == Id);
            if (product == null) return NotFound();
            _products.Remove(product);
            return NoContent();
        }
    }
}