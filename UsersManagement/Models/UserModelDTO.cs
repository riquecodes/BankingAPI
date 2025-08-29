using System.Net.Mail;

namespace UsersManagement.Models
{
    public class UserModelDTO
    {
        public required string Name { get; set; }
        public required string Cpf { get; set; }
        public string? Celphone { get; set; }
        public string? Email { get; set; }

    }
}
