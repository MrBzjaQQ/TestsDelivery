namespace TestsDelivery.DataTransferObjects
{
    /// <summary>
    /// LoginSucceedResponseDto represents Data Transfer Object for response
    /// </summary>
    public class LoginSucceedResponseDto
    {
        public string AccessToken { get; set; }
        
        public string UserName { get; set; }
    }
}