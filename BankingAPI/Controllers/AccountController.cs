using Microsoft.AspNetCore.Mvc;
using BankingAPI.Models;
using BankingAPI.Services;

namespace BankingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("accountId/{id}")]
        public async Task<ActionResult<AccountModel>> GetAccountById(int id)
        {
            var account = await _accountService.GetAccountById(id);

            return Ok(account);
        }

        [HttpGet("accounts/{userId}")]
        public async Task<ActionResult<AccountModel>> GetAccountsByUserId(int userId)
        { 
            var accounts = await _accountService.GetAccountsByUserId(userId);

            return Ok(accounts);

        }

        [HttpGet("{id}/balance")]
        public async Task<ActionResult<decimal>> GetBalanceById(int id)
        {
            var balance = await _accountService.GetBalanceById(id);

            return Ok(balance);
        }

        [HttpPost]
        public async Task<ActionResult<AccountModel>> CreateAccount([FromBody] AccountModel account)
        {
            var newAccount = await _accountService.CreateAccount(account);

            return CreatedAtAction(
                nameof(GetAccountById), 
                new { id = newAccount.Id },
                newAccount);
        }
    }
}
