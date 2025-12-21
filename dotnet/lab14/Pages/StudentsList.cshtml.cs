using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using lab5.Models;

namespace lab14.Pages
{
    public class StudentsListModel : PageModel
    {
        public List<Student>? Students { get; set; }

        public void OnGet()
        {
            var http = new HttpClient();
            http.BaseAddress = new Uri($"{Request.Scheme}://{Request.Host}");

            var response = http.GetAsync("/api/students").GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                Students = JsonSerializer.Deserialize<List<Student>>(
                    json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else
            {
                Students = new List<Student>();
            }
        }
    }
}