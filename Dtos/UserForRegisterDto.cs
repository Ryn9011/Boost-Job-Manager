using System.ComponentModel.DataAnnotations;

namespace JobTracker.API.Dtos
{
    public class UserForRegisterDto
    {        
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 6)]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}