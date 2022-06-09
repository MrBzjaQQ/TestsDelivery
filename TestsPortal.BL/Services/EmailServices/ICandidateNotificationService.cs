using TestsPortal.Domain.Candidates;

namespace TestsPortal.BL.Services.EmailServices
{
    public interface ICandidateNotificationService
    {
        void NotifyCandidate(Candidate candidate, string message);

        void NotifyCandidates(IEnumerable<Candidate> candidates, string message);
    }
}
