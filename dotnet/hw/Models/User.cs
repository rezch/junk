using System.ComponentModel.DataAnnotations;
using BulletinBoard.Login;

namespace BulletinBoard.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; } = LoginRoles.User;
        public string PasswordHash { get; set; }
    }

    public class OperationResult
    {
        public bool Success { get; }
        public string Message { get; }

        public OperationResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public static OperationResult Ok(string message = "") => new OperationResult(true, message);
        public static OperationResult Fail(string message) => new OperationResult(false, message);
    }
}