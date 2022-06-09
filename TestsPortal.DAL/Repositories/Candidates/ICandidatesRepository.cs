using TestsDelivery.DAL.Shared.Repository;

namespace TestsPortal.DAL.Repositories.Candidate
{
    public interface ICandidatesRepository : IBaseRepository<Models.Candidates.Candidate>
    {
        IEnumerable<Models.Candidates.Candidate> GetByOriginalIds(params long[] originalIds);

        void Create(IEnumerable<Models.Candidates.Candidate> candidates);
    }
}
