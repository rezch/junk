using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using lab5.Models;
using System.Text.Json;
using System.Text;

namespace lab14.Pages
{
    public class CreateStudentModel : PageModel
    {
        [BindProperty]
        public Student Student { get; set; } = new Student();

        public string? Message { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var http = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5053/")
            };

            var json = JsonSerializer.Serialize(Student);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = http.PostAsync("/api/students", content).GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                Message = "Студент успешно добавлен!";
                Student = new Student();
                return Page();
            }
            else
            {
                Message = "Ошибка при добавлении студента";
                return Page();
            }
        }
    }
}