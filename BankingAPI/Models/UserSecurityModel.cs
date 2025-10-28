namespace BankingAPI.Models
{
    public class UserSecurityModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required byte[] TransactionPinHash { get; set; }
        public required byte[] TransactionPinSalt { get; set; }
        public int FailedAttempts { get; set; }
        public DateTime? LockedUntil { get; set; }
        
        public UserModel User { get; set; } = null!;
    }
}
