using Microsoft.AspNetCore.Mvc.RazorPages;
using BulletinBoard.Models;
using BulletinBoard.Database;

namespace BulletinBoard.Pages
{
    public class NotesModel : PageModel
    {
        private readonly DatabaseHelper _db;
        public List<Note> Notes { get; set; } = [];
        public string? Search { get; set; }

        public NotesModel(DatabaseHelper db)
        {
            _db = db;
        }

        public void OnGet(string? search)
        {
            Search = search;
            Notes = _db.GetNotes(search ?? "");
        }
    }
}