using AutoMapper;
using TestsPortal.DAL.Data;

namespace TestsPortal.DAL.Repositories.Candidate
{
    public class CandidatesRepository : BaseRepository<Models.Candidates.Candidate>, ICandidatesRepository
    {
        public CandidatesRepository(TestsPortalContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public void Create(IEnumerable<Models.Candidates.Candidate> candidates)
        {
            Context.Candidates.AddRange(candidates);
            Context.SaveChanges();
        }

        public IEnumerable<Models.Candidates.Candidate> GetByOriginalIds(params long[] originalIds)
        {
            return Context.Candidates.Where(candidate => originalIds.Contains(candidate.OriginalId)).ToList();
        }
    }
}
