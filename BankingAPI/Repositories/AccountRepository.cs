using BankingAPI.Context;
using BankingAPI.Models;

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

        public async Task<AccountModel> CreateAccount(AccountModel account)
        { 
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
            return account;
        }

    }
}
