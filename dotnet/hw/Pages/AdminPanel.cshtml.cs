using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BulletinBoard.Models;
using BulletinBoard.Database;

namespace BulletinBoard.Pages
{
    public class AdminPanelModel : PageModel
    {
        private readonly DatabaseHelper _db;

        public List<Note> Notes { get; set; } = [];
        public string? Message { get; set; }

        public AdminPanelModel(DatabaseHelper db)
        {
            _db = db;
        }

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("role");
            if (role != "admin")
                return RedirectToPage("/Index");

            Notes = _db.GetNotes(null);
            return Page();
        }

        public IActionResult OnPostDelete(int id)
        {
            var role = HttpContext.Session.GetString("role");
            if (role != "admin")
                return RedirectToPage("/Index");

            if (_db.DeleteNote(id))
                Message = "Объявление удалено";
            else
                Message = "Ошибка удаления";
            Notes = _db.GetNotes(null);
            return Page();
        }
    }
}