using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BulletinBoard.Models;
using BulletinBoard.Database;

namespace BulletinBoard.Pages
{
    public class MyNotesModel : PageModel
    {
        private readonly DatabaseHelper _db;
        public List<Note> Notes { get; set; } = [];

        public string? Message { get; set; }

        public MyNotesModel(DatabaseHelper db)
        {
            _db = db;
        }

        public IActionResult OnGet()
        {
            var username = HttpContext.Session.GetString("username");
            if (string.IsNullOrEmpty(username))
                return RedirectToPage("/Index");

            Notes = _db.GetNotes(null).Where(n => n.Owner == username).ToList();
            return Page();
        }

        public IActionResult OnPostDelete(int id)
        {
            var username = HttpContext.Session.GetString("username");
            if (string.IsNullOrEmpty(username))
                return RedirectToPage("/Index");

            var note = _db.GetNoteById(id);
            if (note == null || note.Owner != username)
            {
                Message = "Нет доступа к удалению этого объявления.";
            }
            else if (_db.DeleteNote(id))
            {
                Message = "Объявление удалено!";
            }
            else
            {
                Message = "Ошибка при удалении.";
            }

            Notes = _db.GetNotes(null).Where(n => n.Owner == username).ToList();
            return Page();
        }
    }
}