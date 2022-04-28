using System.Collections.Generic;

namespace TestsDelivery.DAL.Repositories.Candidate
{
    public interface ICandidateRepository: IBaseRepository<Models.Candidate.Candidate>
    {
        IEnumerable<Models.Candidate.Candidate> GetCandidates(IEnumerable<long> ids);
    }
}
