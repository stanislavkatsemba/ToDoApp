using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain.ToDoItems;
using ToDoApp.Domain.Users;

namespace ToDoApp.Data
{
    public class ToDoAppContext : DbContext
    {
        public ToDoAppContext()
        {

        }

        public ToDoAppContext(DbContextOptions<ToDoAppContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=ToDoApp;Integrated Security=True");
            }
#endif
        }

        public DbSet<UserSnapshot> Users { get; set; }

        public DbSet<ToDoItemSnapshot> ToDoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserSnapshot>(entity =>
            {
                entity.HasIndex(x => x.Name).IsUnique();
            });
        }
    }
}
