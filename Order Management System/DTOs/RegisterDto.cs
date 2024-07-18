using Order_Management_System.Contants;
using System.ComponentModel.DataAnnotations;

namespace Order_Management_System.DTOs
{
    public class RegisterDto
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }

        public string Role { get; set; }



    }
}
