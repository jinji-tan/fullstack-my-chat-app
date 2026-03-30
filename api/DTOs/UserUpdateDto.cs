using System.ComponentModel.DataAnnotations;

namespace api.DTOs
{
    public class UserUpdateDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = "";

        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
    }
}