using Microsoft.EntityFrameworkCore;
using BankingAPI.Models;

namespace BankingAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<UserModel> Users { get; set; } = null!;
    }
}
