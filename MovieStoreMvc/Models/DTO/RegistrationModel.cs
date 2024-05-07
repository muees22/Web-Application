using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MovieStoreMvc.Models.DTO
{
    public class RegistrationModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [Required]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[@$!%*#?&])[A-Za-z\\d@$!%*#?&]{6,}$", ErrorMessage= "Minimum six characters, at least one uppercase letter, one lowercase letter and one number:")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        [DisplayName("Password Confirm")]
        public string PasswordConfirm { get; set; }
        public string? Role { get; set; }

    }
}
