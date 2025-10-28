using Microsoft.EntityFrameworkCore;
using BankingAPI.Models;

namespace BankingAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<UserModel> Users { get; set; } = null!;
        public DbSet<AccountModel> Accounts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // configure one-to-many relationship between User and Account
            modelBuilder.Entity<AccountModel>()
                .HasOne(a => a.User)
                .WithMany(u => u.Accounts)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade); // if user deleted, delete accounts

            // map enum to int in database
            modelBuilder.Entity<AccountModel>()
                .Property(a => a.AccountType)
                .HasConversion<int>();

            // configure one-to-one relationship between User and UserSecurity
            modelBuilder.Entity<UserModel>()
                .HasOne(u => u.UserSecurity)
                .WithOne(s => s.User)
                .HasForeignKey<UserSecurityModel>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
