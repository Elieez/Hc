using System;
namespace HealthCareABApi.DTO
{
    public class RegisterDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }
    }
}