namespace BankingAPI.Models
{
    public class AccountModelDTO
    {
        public decimal Balance { get; set; }
        public required string AccountNumber { get; set; }
        public AccountType AccountType { get; set; } = AccountType.Checking;
        public string Agency { get; set; } = "0001";
    }
}
