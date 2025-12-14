using System.Data.SQLite;
using BulletinBoard.Login;
using BulletinBoard.Models;

namespace BulletinBoard.Database
{
    public class DatabaseHelper(ServiceContext context, IHttpContextAccessor httpContextAccessor)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ServiceContext _context = context;

        private bool ActionsAllowed(Note note)
        {
            var role = _httpContextAccessor.HttpContext?.Session.GetString("role");
            var username = _httpContextAccessor.HttpContext?.Session.GetString("username");
            return role == LoginRoles.Admin
                || (role == "user" && note.Owner == username);
        }

        public void LogAction(string description)
        {
            _context.Logs.Add(new ActionLog
            {
                Time = DateTime.Now,
                Description = description
            });
            _context.SaveChanges();
        }

        public List<ActionLog> GetLogs()
        {
            return [.. _context.Logs.OrderByDescending(l => l.Time)];
        }

        public OperationResult UserRegister(User user)
        {
            if (user.Role == LoginRoles.Admin)
            {
                return OperationResult.Fail("Регистрация с ролью администратора запрещена");
            }
            if (_context.Users.Any(u => u.UserName == user.UserName))
            {
                return OperationResult.Fail("Пользователь с таким именем уже существует");
            }
            // хешируем пароль для нового пользователя перед сохранением в бд
            user.PasswordHash = PasswordHasher.HashPassword(user.PasswordHash);
            _context.Users.Add(user);
            _context.SaveChanges();
            LogAction($"Пользователь {user.UserName} зарегистрирован.");
            return OperationResult.Ok("Регистрация успешна");
        }

        public User? GetUserByName(string userName)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == userName);
        }

        public void UserChangePassword(User user, string newPassword)
        {
            user.PasswordHash = PasswordHasher.HashPassword(newPassword);
            _context.SaveChanges();
        }

        public List<Note> GetNotes(string search)
        {
            var query = _context.Notes.AsQueryable();

            if (search != null)
            {
                query = query.Where(
                    p => p.Name.ToLower().Contains(search.ToLower()));
            }

            return [.. query.OrderByDescending(p => p.CreationDate)];
        }

        public Note? GetNoteById(int id)
        {
            return _context.Notes.FirstOrDefault(p => p.Id == id);
        }

        public bool AddNote(Note note)
        {
            try
            {
                note.CreationDate = DateTime.Now;
                _context.Notes.Add(note);
                _context.SaveChanges();
                LogAction($"Создано объявление id={note.Id}.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public bool UpdateNote(Note note)
        {
            var noteFromDB = GetNoteById(note.Id);
            if (noteFromDB == null || !ActionsAllowed(noteFromDB))
            {
                return false;
            }

            noteFromDB.Price = note.Price;
            noteFromDB.Name = note.Name;
            noteFromDB.Description = note.Description;
            _context.SaveChanges();
            LogAction($"Изменено объявление id={note.Id}.");
            return true;
        }

        public bool DeleteNote(int id)
        {
            var noteFromDB = GetNoteById(id);
            if (noteFromDB == null || !ActionsAllowed(noteFromDB))
            {
                return false;
            }

            _context.Notes.Remove(noteFromDB);
            _context.SaveChanges();
            LogAction($"Удалено объявление id={id}.");
            return true;
        }
    }
}