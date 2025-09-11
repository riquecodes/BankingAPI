namespace BankingAPI.Models
{
    public class UserModelDTO
    {
        public required string Name { get; set; }
        public required string Cpf { get; set; }
        public required string Password { get; set; }
        public string? Celphone { get; set; }
        public string? Email { get; set; }
        public string Role { get; set; } = "client";

    }
}
