using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BulletinBoard.Database;
using BulletinBoard.Models;
using BulletinBoard.Login;

namespace BulletinBoard.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ServiceContext _context;
        private readonly DatabaseHelper _db;

        public string? Username { get; set; }
        public string? Role { get; set; }
        public string? LoginError { get; set; }

        public IndexModel(ServiceContext context, DatabaseHelper db)
        {
            _context = context;
            _db = db;
        }

        public void OnGet()
        {
            Username = HttpContext.Session.GetString("username");
            Role = HttpContext.Session.GetString("role");
        }

        public IActionResult OnPost()
        {
            string? username = Request.Form["username"];
            string? password = Request.Form["password"];

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                LoginError = "Введите логин и пароль";
                return Page();
            }

            var user = _context.Users.FirstOrDefault(u => u.UserName == username);

            if (user != null && PasswordHasher.VerifyPassword(password, user.PasswordHash))
            {
                HttpContext.Session.SetString("username", username);
                HttpContext.Session.SetString("role", user.Role);
                _db.LogAction($"Пользователь {username} вошёл в систему.");
                return RedirectToPage("/Index");
            }
            LoginError = "Неверный логин или пароль";
            return Page();
        }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage();
        }
    }
}
