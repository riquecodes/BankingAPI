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
        [HttpPost("{accountId}/set-transaction-pin")]
        public async Task<ActionResult> SetTransactionPin(int accountId, [FromBody] TransactionPasswordDTO transactionPin)
        {
            await _accountService.SetTransactionPin(accountId, transactionPin.TemporaryPassword);
            return Ok(new { message = "Transaction Password registered succesfully!" });
        }
    }
}
