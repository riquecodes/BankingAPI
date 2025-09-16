﻿namespace BankingAPI.Models
{
    public class RegisterDTO
    {
        public required string Name { get; set; }
        public required string Cpf { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public string? Celphone { get; set; }
        public string Role { get; set; } = "client";
    }
}
