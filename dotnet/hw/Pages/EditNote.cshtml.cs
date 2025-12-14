using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BulletinBoard.Database;
using BulletinBoard.Models;

namespace BulletinBoard.Pages
{
    public class EditNoteModel : PageModel
    {
        private readonly DatabaseHelper _db;

        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }
        public Note? Note { get; set; }

        [BindProperty]
        public Note Input { get; set; } = new();

        public EditNoteModel(DatabaseHelper db)
        {
            _db = db;
        }

        public IActionResult OnGet(int? id)
        {
            var username = HttpContext.Session.GetString("username");
            if (!id.HasValue)
            {
                ErrorMessage = "Некорректный ID объявления.";
                return Page();
            }

            var note = _db.GetNoteById(id.Value);
            if (note == null || note.Owner != username)
            {
                ErrorMessage = "Объявление не найдено или нет прав на редактирование.";
                return Page();
            }

            Note = note;
            // Для формы
            Input.Id = note.Id;
            Input.Name = note.Name;
            Input.Price = note.Price;
            Input.Description = note.Description;
            return Page();
        }

        public IActionResult OnPost()
        {
            var username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                ErrorMessage = "Необходимо войти в систему.";
                return Page();
            }

            var noteInDb = _db.GetNoteById(Input.Id);
            if (noteInDb == null || noteInDb.Owner != username)
            {
                ErrorMessage = "Нет доступа к редактированию этого объявления.";
                return Page();
            }

            noteInDb.Name = Input.Name;
            noteInDb.Price = Input.Price;
            noteInDb.Description = Input.Description;

            if (_db.UpdateNote(noteInDb))
            {
                SuccessMessage = "Объявление обновлено!";
                Note = noteInDb;
            }
            else
            {
                ErrorMessage = "Ошибка при обновлении объявления.";
            }
            return Page();
        }
    }
}