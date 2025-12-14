using BulletinBoard.Models;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.Database
{
    public class ServiceContext : DbContext
    {
        private readonly string _DbSource = "Data Source=notes.db";

        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<ActionLog> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_DbSource);
        }
    }
}