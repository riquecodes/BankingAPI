namespace BankingAPI.Models
{
    public class AccountSecurityModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string TransactionPinHash { get; set; } = null!;
        public string TransactionPinSalt { get; set; } = null!;
        public int FailedAttempts { get; set; }
        public DateTime? LockedUntil { get; set; }
        
        public AccountModel Account { get; set; } = null!;
    }
}
