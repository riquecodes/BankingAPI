namespace BankingAPI.Models
{
    public class LoginDTO 
    {
        public required string Cpf { get; set; }
        public required string Password { get; set; }
    }
}
