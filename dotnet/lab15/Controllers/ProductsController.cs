using lab15.Database;
using lab15.Models;
using Microsoft.AspNetCore.Mvc;


namespace lab15.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateProductController : ControllerBase
    {
        private readonly DatabaseHelper _dbHelper = new();

        [HttpGet]
        public ActionResult<IEnumerable<CreateProductResponse>> GetAll()
        {
            return _dbHelper.GetProducts();
        }

        [HttpGet("{id}")]
        public ActionResult<CreateProductResponse> GetById(int id)
        {
            var product = _dbHelper.GetProductById(id);
            return product != null
                ? (ActionResult<CreateProductResponse>)product
                : NotFound();
        }

        [HttpPost]
        public ActionResult<CreateProductResponse> Create(CreateProductRequest request)
        {
            long newId = _dbHelper.AddProduct(request);
            return newId != -1
                ? Ok(newId)
                : StatusCode(500, "При добавлении произошла ошибка");
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CreateProductRequest request)
        {
            return _dbHelper.UpdateProductById(id, request)
                ? Ok()
                : NoContent();
        }

        [HttpDelete]
        public IActionResult Delete(CreateProductRequest request)
        {
            return _dbHelper.DeleteProduct(request)
                ? Ok()
                : NotFound();
        }

        [HttpDelete("id")]
        public IActionResult DeleteById(int Id)
        {
            return _dbHelper.DeleteProductById(Id)
                ? Ok()
                : NotFound();
        }
    }
}