namespace TestsPortal.BL.Services.EmailServices
{
    public interface IEmailService
    {
        public bool SendEmail(string receiverEmail, string content);
    }
}
