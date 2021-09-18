using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.DataTransferObjects
{
    /// <summary>
    /// LoginRequestDto represents Data Transfer Object received from client
    /// </summary>
    public class LoginRequestDto
    {
        [Required]
        public string UserName { get; set; }

        [MinLength(6)]
        public string Password { get; set; }
    }
}
