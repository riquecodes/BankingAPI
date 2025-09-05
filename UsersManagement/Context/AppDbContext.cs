using Microsoft.EntityFrameworkCore;
using UsersManagement.Models;

namespace UsersManagement.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<UserModel> Users { get; set; } = null!;
    }
}
