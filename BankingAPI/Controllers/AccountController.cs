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

        [HttpPost("{accountId}/set-transaction-password")]
        public async Task<ActionResult> SetTransactionPassword(int accountId, [FromBody] TransactionPasswordDTO transactionPassword)
        {
            await _accountService.SetTransactionPassword(accountId, transactionPassword.TemporaryPassword);
            return Ok(new { message = "Transaction Password registered succesfully!" });
        }
    }
}
