namespace MusicSystem.DataAccess
{
    using MusicSystem.Models;
    using System.Data.Entity;

    public class DataContext : DbContext
    {
        public DataContext()
            : base("name=DataContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Migrations.Configuration>());
        }
        
        public virtual DbSet<Song> Songs { get; set; }
        public virtual DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>().HasIndex(g => g.Name).IsUnique();
            modelBuilder.Entity<Song>().HasIndex(s => s.Name).IsUnique();
        }
    }
}