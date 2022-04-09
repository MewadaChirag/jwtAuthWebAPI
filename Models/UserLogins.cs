using System.ComponentModel.DataAnnotations;

namespace jwtAuthWebAPI.Models
{
    public class UserLogins
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        
       
    }
}
