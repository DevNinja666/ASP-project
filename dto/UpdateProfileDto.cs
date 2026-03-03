namespace Invoicer.DTOs.User
{
   
    public class UpdateProfileDto
    {
        
        public string Name { get; set; } = null!;
        public string? Address { get; set; }

        
        public string Email { get; set; } = null!;

        public string? PhoneNumber { get; set; }
    }
}
