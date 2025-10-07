using BankingAPI.Context;
using BankingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingAPI.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AccountModel?> GetAccountById(int id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<IEnumerable<AccountModel>> GetAccountsByUserId(int userId)
        {
            return await _context.Accounts
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task<decimal> GetBalanceById(int id)
        {
            var balance = await _context.Accounts
                .Where(a => a.Id == id)
                .Select(a => a.Balance)
                .FirstOrDefaultAsync();

            return balance;
        }

        public async Task<AccountModel> CreateAccount(AccountModel account)
        { 
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
            return account;
        }

    }
}