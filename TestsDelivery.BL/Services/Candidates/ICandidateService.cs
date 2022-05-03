using System.Collections.Generic;
using TestsDelivery.Domain.Candidate;
using TestsDelivery.Domain.Lists;

namespace TestsDelivery.BL.Services.Candidates
{
    public interface ICandidateService
    {
        Candidate CreateCandidate(Candidate candidate);

        Candidate GetCandidate(long id);

        IEnumerable<Candidate> GetCandidates(IEnumerable<long> candidateIds);

        void EditCandidate(Candidate candidate);

        IEnumerable<Candidate> GetList(ListFilter filter);
    }
}
