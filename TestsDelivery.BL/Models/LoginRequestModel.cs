using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.BL.Models
{
    /// <summary>
    /// LoginRequestDto represents Data Transfer Object received from client
    /// </summary>
    public class LoginRequestModel
    {
        [Required]
        public string UserName { get; set; }

        [MinLength(6)]
        public string Password { get; set; }
    }
}
