using System.ComponentModel.DataAnnotations;

namespace UserServices.DTO
{
    public class UserDTO
    {
        public class LoginDto
        {
            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid Email Format")]
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class LoginResponseDto
        {
            public string Token { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
        }

        public class SignupDto
        {
            public string UserName { get; set; }

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid Email Format")]
            public string UserEmail { get; set; }

            [Required(ErrorMessage = "Contact Number is required")]
            [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid contact number. It must be a 10-digit number.")]
            public string UserContactNumber { get; set; }
            
            public string Password { get; set; }
            
            public string Role { get; set; } = "User";
        }
    }
}
