using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BulletinBoard.Database;
using BulletinBoard.Login;
using BulletinBoard.Models;

namespace BulletinBoard.Pages
{
    public class ChangePasswordModel : PageModel
    {
        private readonly DatabaseHelper _db;

        public ChangePasswordModel(DatabaseHelper db) => _db = db;

        [BindProperty]
        public string OldPassword { get; set; }
        [BindProperty]
        public string NewPassword { get; set; }
        [BindProperty]
        public string NewPasswordRepeat { get; set; }
        public string? Username { get; set; }
        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
            Username = HttpContext.Session.GetString("username");
        }

        public void OnPost()
        {
            Username = HttpContext.Session.GetString("username");
            if (Username == null)
            {
                ErrorMessage = "Вы не авторизованы.";
                return;
            }
            if (string.IsNullOrEmpty(OldPassword) || string.IsNullOrEmpty(NewPassword) || string.IsNullOrEmpty(NewPasswordRepeat))
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

            var user = _db.GetUserByName(Username);
            if (user == null)
            {
                ErrorMessage = "Пользователь не найден.";
                return;
            }
            if (!PasswordHasher.VerifyPassword(OldPassword, user.PasswordHash))
            {
                ErrorMessage = "Неверный текущий пароль.";
                return;
            }
            // Используем DatabaseHelper для смены пароля
            _db.UserChangePassword(user, NewPassword);
            Message = "Пароль успешно изменён.";
        }
    }
}