namespace BulletinBoard.Models
{
    public class ActionLog
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string? Description { get; set; }
    }
}