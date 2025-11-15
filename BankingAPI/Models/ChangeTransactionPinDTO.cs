namespace BankingAPI.Models
{
    public class ChangeTransactionPinDTO
    {
        public required string CurrentPin { get; set; }
        public required string NewPin { get; set; }
    }
}
