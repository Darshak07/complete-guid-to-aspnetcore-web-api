using System.ComponentModel.DataAnnotations;

namespace my_books.Data.ViewModels.Authentication
{
    public class RegisterVM
    {
        [Required(ErrorMessage ="UserName is Required")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Role is Required")]
        public string? Role { get; set; }
    }
}
