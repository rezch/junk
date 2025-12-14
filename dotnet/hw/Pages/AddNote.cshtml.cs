using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BulletinBoard.Models;
using BulletinBoard.Database;

namespace BulletinBoard.Pages
{
    public class AddNoteModel : PageModel
    {
        private readonly DatabaseHelper _db;

        public string? Username { get; set; }
        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }
        [BindProperty]
        public NoteRequest Input { get; set; } = new();

        public AddNoteModel(DatabaseHelper db)
        {
            _db = db;
        }

        public void OnGet()
        {
            Username = HttpContext.Session.GetString("username");
        }

        public IActionResult OnPost()
        {
            Username = HttpContext.Session.GetString("username");
            if (Username == null)
            {
                ErrorMessage = "Необходимо войти в систему.";
                return Page();
            }
            if (string.IsNullOrWhiteSpace(Input.Name))
            {
                ErrorMessage = "Название обязательно.";
                return Page();
            }
            var note = new Note
            {
                Name = Input.Name,
                Price = Input.Price,
                Description = Input.Description,
                Owner = Username
            };

            if (_db.AddNote(note))
            {
                SuccessMessage = "Объявление добавлено!";
                Input = new();
                return Page();
            }
            else
            {
                ErrorMessage = "Ошибка при добавлении объявления.";
                return Page();
            }
        }
    }
}