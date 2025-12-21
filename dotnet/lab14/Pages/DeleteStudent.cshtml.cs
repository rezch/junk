using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Net.Http;

namespace lab14.Pages
{
    public class DeleteStudentModel : PageModel
    {
        [BindProperty]
        public int StudentId { get; set; }
        public string? Message { get; set; }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            var http = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5053/")
            };

            var response = http.DeleteAsync($"/api/students/{StudentId}").GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                Message = "Студент успешно удалён!";
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Message = "Студент с таким Id не найден.";
            }
            else
            {
                Message = "Ошибка при удалении.";
            }
            return Page();
        }
    }
}