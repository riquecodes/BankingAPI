namespace BankingAPI.Models
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Cpf { get; set; }
        public string? Celphone { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
