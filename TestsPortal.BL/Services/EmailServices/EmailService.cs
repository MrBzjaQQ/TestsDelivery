using TestsDelivery.Infrastructure.Logging;

namespace TestsPortal.BL.Services.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly IAppLogging<EmailService> _logger;

        public EmailService(IAppLogging<EmailService> logger)
        {
            _logger = logger;
        }
        public bool SendEmail(string receiverEmail, string content)
        {
            _logger.LogAppInformation($"Sending email to {receiverEmail} with content:\n{content}");
            return true;
        }
    }
}
