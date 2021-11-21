using TestsDelivery.Domain.Candidate;

namespace TestsDelivery.BL.Services.Candidates
{
    public interface ICandidateService
    {
        Candidate CreateCandidate(Candidate candidate);

        Candidate GetCandidate(long id);

        void EditCandidate(Candidate candidate);
    }
}
