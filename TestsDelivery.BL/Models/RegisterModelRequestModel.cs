using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.BL.Models
{
    public class RegisterModelRequestModel
    {
        [Required]
        public string UserName { get; set; }

        [MinLength(6)]
        public string Password { get; set; }

        [MinLength(6)]
        [Compare(nameof(Password))]
        public string PasswordConfirm { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
