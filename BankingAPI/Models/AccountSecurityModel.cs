namespace BankingAPI.Models
{
    public class AccountSecurityModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public required byte[] TransactionPinHash { get; set; }
        public required byte[] TransactionPinSalt { get; set; }
        public int FailedAttempts { get; set; }
        public DateTime? LockedUntil { get; set; }
        
        public AccountModel Account { get; set; } = null!;
    }
}
