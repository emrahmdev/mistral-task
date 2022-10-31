using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
            modelBuilder.Entity<Permission>().HasQueryFilter(p => !p.IsDeleted);
        }

        public override int SaveChanges()
        {

            var entries = ChangeTracker
                    .Entries()
                    .Where(e => e.Entity is BaseModel && (
                            e.State == EntityState.Added
                            || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseModel)entityEntry.Entity).UpdatedDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseModel)entityEntry.Entity).CreatedDate = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }
    }
}
