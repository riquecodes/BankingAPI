namespace BankingAPI.Models
{
    public enum AccountType
    {
        Checking = 1,
        Savings = 2,
        Investment = 3
    }

    public class AccountModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Balance { get; set; }
        public required string AccountNumber { get; set; }
        public AccountType AccountType { get; set; } = AccountType.Checking;
        public bool IsActive { get; set; } = true;
        public string Agency { get; set; } = "0001";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public UserModel User { get; set; } = null!;
        public AccountSecurityModel? AccountSecurity { get; set; }
    }
}
