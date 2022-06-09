using TestsDelivery.Infrastructure.Logging;
using TestsPortal.Domain.Candidates;

namespace TestsPortal.BL.Services.EmailServices
{
    public class CandidateNotificationService : ICandidateNotificationService
    {
        private readonly IAppLogging<EmailService> _logger;
        private readonly IEmailService _emailService;

        public CandidateNotificationService(
            IEmailService emailService,
            IAppLogging<EmailService> logger)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public void NotifyCandidate(Candidate candidate, string message)
        {
            _emailService.SendEmail(candidate.Email, message);
        }

        public void NotifyCandidates(IEnumerable<Candidate> candidates, string message)
        {
            foreach (var candidate in candidates)
            {
                _emailService.SendEmail(candidate.Email, message);
            }
        }
    }
}
