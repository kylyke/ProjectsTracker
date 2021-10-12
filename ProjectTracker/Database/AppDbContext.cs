using Microsoft.EntityFrameworkCore;
using ProjectTracker.Database.Concrete;

namespace ProjectTracker.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Demand>()
        //        .HasRequired(s => s.User)
        //        .WithMany()
        //        .WillCascadeOnDelete(false);
        //}
        public virtual DbSet<Demand> Demands { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Support> Supports { get; set; }
        public virtual DbSet<User> Users { get; set; }

    }
}
