using lab5.Database;
using lab5.Models;
using Microsoft.AspNetCore.Mvc;

namespace lab5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly DatabaseHelper _dbHelper = new();

        [HttpGet]
        public IEnumerable<Student> Get()
        {
            try {
                return _dbHelper.GetStudents();
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
            return null;
        }

        [HttpPost]
        public IActionResult Post(Student student)
        {
            return _dbHelper.AddStudent(student)
                ? Ok()
                : StatusCode(500, "При добавлении произошла ошибка");
        }
    }
}