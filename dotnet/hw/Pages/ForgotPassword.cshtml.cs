using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BulletinBoard.Database;
using BulletinBoard.Login;
using BulletinBoard.Models;

namespace BulletinBoard.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly DatabaseHelper _db;

        public ForgotPasswordModel(DatabaseHelper db) => _db = db;

        [BindProperty]
        public string UserName { get; set; } = "";
        [BindProperty]
        public string Email { get; set; } = "";
        [BindProperty]
        public string NewPassword { get; set; }
        [BindProperty]
        public string NewPasswordRepeat { get; set; }

        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }

        public void OnGet() { }

        public void OnPost()
        {
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Email) || 
                string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(NewPasswordRepeat))
            {
                ErrorMessage = "Все поля обязательны.";
                return;
            }
            if (NewPassword.Length < 5)
            {
                ErrorMessage = "Пароль должен быть длиной не менее 5 символов.";
                return;
            }
            if (NewPassword != NewPasswordRepeat)
            {
                ErrorMessage = "Пароли не совпадают.";
                return;
            }

            var user = _db.GetUserByName(UserName);
            if (user == null)
            {
                ErrorMessage = "Пользователь не найден.";
                return;
            }

            // email никак не используется, только логин проверяется
            // Меняем пароль через твой DatabaseHelper
            _db.UserChangePassword(user, NewPassword);
            Message = "Пароль успешно сброшен. Теперь Вы можете войти с новым паролем.";
        }
    }
}