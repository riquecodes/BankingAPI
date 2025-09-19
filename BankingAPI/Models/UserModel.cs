namespace BankingAPI.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Cpf { get; set; }
        public string? Celphone { get; set; }
        public string? Email { get; set; }
        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }
        public string Role { get; set; } = "client";
        public bool IsActive { get; set; } = true;
        public bool TemporaryPassword { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
