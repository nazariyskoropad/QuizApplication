using QuizApplication.Contracts.Constants;
using System.ComponentModel.DataAnnotations;

namespace QuizApplication.Contracts.DTOs
{
    public class AdminLoginDto
    {
        [Required]
        [EmailAddress]
        [StringLength(PropertyConstrains.EmailLength)]
        public string Email { get; set; }

        [Required]
        [StringLength(PropertyConstrains.PasswordLength)]
        public string Password { get; set; }
    }
}
