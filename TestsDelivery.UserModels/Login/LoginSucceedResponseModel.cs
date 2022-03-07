namespace TestsDelivery.UserModels.Login
{
    /// <summary>
    /// LoginSucceedResponseDto represents Data Transfer Object for response
    /// </summary>
    public class LoginSucceedResponseModel
    {
        public string AccessToken { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;
    }
}