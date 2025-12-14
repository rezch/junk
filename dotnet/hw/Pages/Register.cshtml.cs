using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BulletinBoard.Login;
using BulletinBoard.Database;
using BulletinBoard.Models;

namespace BulletinBoard.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly DatabaseHelper _db;

        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }

        [BindProperty]
        public RegisterRequest Input { get; set; } = new();

        public RegisterModel(DatabaseHelper db)
        {
            _db = db;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(Input.UserName) ||
                string.IsNullOrWhiteSpace(Input.Password) ||
                string.IsNullOrWhiteSpace(Input.ConfirmPassword))
            {
                ErrorMessage = "Заполните все поля";
                return Page();
            }

            if (Input.Password != Input.ConfirmPassword)
            {
                ErrorMessage = "Пароли не совпадают";
                return Page();
            }

            if (Input.Password.Length < 4)
            {
                ErrorMessage = "Пароль слишком короткий";
                return Page();
            }

            var user = new User
            {
                UserName = Input.UserName,
                PasswordHash = Input.Password,
                Role = LoginRoles.User
            };

            // хешируется внутри UserRegister
            var result = _db.UserRegister(user);
            if (!result.Success)
            {
                ErrorMessage = result.Message;
                return Page();
            }

            return RedirectToPage("/Index");
        }

        public class RegisterRequest
        {
            public string? UserName { get; set; }
            public string? Password { get; set; }
            public string? ConfirmPassword { get; set; }
        }
    }
}