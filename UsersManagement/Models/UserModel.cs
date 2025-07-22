namespace UsersManagement.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Celphone { get; set; }
        public string? Email { get; set; }
    }
}
