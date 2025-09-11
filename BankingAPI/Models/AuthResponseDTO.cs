namespace BankingAPI.Models
{
    public class AuthResponseDTO
    {
        public required string Token { get; set; }
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Cpf { get; set; }
        public required string Role { get; set; }
    }
}
