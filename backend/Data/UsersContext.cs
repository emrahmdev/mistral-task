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
    }
}
