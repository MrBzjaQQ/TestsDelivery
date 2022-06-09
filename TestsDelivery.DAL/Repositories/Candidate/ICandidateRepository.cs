using System.Collections.Generic;
using TestsDelivery.DAL.Shared.Repository;

namespace TestsDelivery.DAL.Repositories.Candidate
{
    public interface ICandidateRepository: IBaseRepository<Models.Candidate.Candidate>
    {
        IEnumerable<Models.Candidate.Candidate> GetCandidates(IEnumerable<long> ids);
    }
}
