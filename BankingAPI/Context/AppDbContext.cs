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

            // configure one-to-one relationship between Account and AccountSecurity
            modelBuilder.Entity<AccountSecurityModel>()
                .HasOne(s => s.Account)
                .WithOne(a => a.AccountSecurity)
                .HasForeignKey<AccountSecurityModel>(s => s.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
