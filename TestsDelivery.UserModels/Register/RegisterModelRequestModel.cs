using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.UserModels.Register
{
    public class RegisterModelRequestModel
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [MinLength(6)]
        [Compare(nameof(Password))]
        public string PasswordConfirm { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;
    }
}
