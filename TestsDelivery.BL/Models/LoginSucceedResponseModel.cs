namespace TestsDelivery.BL.Models
{
    /// <summary>
    /// LoginSucceedResponseDto represents Data Transfer Object for response
    /// </summary>
    public class LoginSucceedResponseModel
    {
        public string AccessToken { get; set; }
        
        public string UserName { get; set; }
    }
}