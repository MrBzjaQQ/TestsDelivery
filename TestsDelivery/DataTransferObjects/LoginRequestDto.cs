using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.DataTransferObjects
{
    public class LoginRequestDto
    {
        [Required]
        public string UserName { get; set; }

        [MinLength(6)]
        public string Password { get; set; }
    }
}
