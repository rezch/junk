using System.ComponentModel.DataAnnotations;

namespace BulletinBoard.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public DateTime CreationDate { get; set; }

        public decimal? Price { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }

    public class NoteRequest
    {
        public decimal? Price { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }

    public class NotePatchRequest
    {
        public decimal? Price { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class NoteResponse
    {
        public int StatusCode { get; set; }
        public NoteRequest? Note { get; set; }
    }
}