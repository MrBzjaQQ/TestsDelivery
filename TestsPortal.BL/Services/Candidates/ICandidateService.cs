using TestsPortal.Domain.Candidates;

namespace TestsPortal.BL.Services.Candidates
{
    public interface ICandidateService
    {
        public Candidate CreateCandidate(Candidate candidate);

        public IEnumerable<Candidate> CreateCandidates(IEnumerable<Candidate> candidates);
    }
}
