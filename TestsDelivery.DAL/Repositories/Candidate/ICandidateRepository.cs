using System.Collections.Generic;

namespace TestsDelivery.DAL.Repositories.Candidate
{
    public interface ICandidateRepository
    {
        Models.Candidate.Candidate GetCandidate(long id);

        IEnumerable<Models.Candidate.Candidate> GetCandidates(IEnumerable<long> ids);

        void CreateCandidate(Models.Candidate.Candidate candidate);

        void EditCandidate(Models.Candidate.Candidate candidate);
    }
}
