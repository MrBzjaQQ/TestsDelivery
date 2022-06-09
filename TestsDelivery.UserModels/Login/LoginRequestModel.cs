using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.UserModels.Login
{
    /// <summary>
    /// LoginRequestDto represents Data Transfer Object received from client
    /// </summary>
    public class LoginRequestModel
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }
}
