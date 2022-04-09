using Microsoft.AspNetCore.Identity;

namespace jwtAuthWebAPI.Data
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenValidity { get; set; }
    }
}
