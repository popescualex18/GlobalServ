using Swashbuckle.Examples;
using System.ComponentModel.DataAnnotations;

namespace GlobalServ.DomainModels
{
    public class LoginUserDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\W).*$", ErrorMessage = "Password must have at least one uppercase letter and one non-alphanumeric character")]
        public string Password { get; set; }
    }

    
    public class LoginUserDtoExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new LoginUserDto
            {
                Email = "user@example.com",
                Password = "MyCustomPassword!123",
            };
        }
    }
}
