using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace lab14.Pages
{
    public class HelloModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string UserName { get; set; }
        public string HelloMessage { get; private set; }

        public void OnGet()
        {
            if (!string.IsNullOrEmpty(UserName))
            {
                // Если пользователь передал свое имя — формируем приветствие
                HelloMessage = $"Здравствуйте, {UserName}";
            }
            else
            {
                // Если имя не было передано — выводим подсказку
                HelloMessage = "Пожалуйста, введите ваше имя через URL: /Hello?username=ВашеИмя";
            }
        }
    }
}