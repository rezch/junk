using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BulletinBoard.Database;
using BulletinBoard.Models;

namespace BulletinBoard.Pages
{
    public class ActionLogModel : PageModel
    {
        private readonly DatabaseHelper _db;
        public List<ActionLog> Logs { get; set; } = [];

        public ActionLogModel(DatabaseHelper db)
        {
            _db = db;
        }

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("role");
            if (role != "admin")
                return RedirectToPage("/Index");

            Logs = _db.GetLogs();
            return Page();
        }
    }
}