using TestsPortal.DAL.Data;

namespace TestsPortal.DAL.Repositories.Candidate
{
    public class CandidatesRepository : ICandidatesRepository
    {
        private TestsPortalContext _context;

        public CandidatesRepository(TestsPortalContext context)
        {
            _context = context;
        }

        public void CreateCandidate(Models.Candidates.Candidate candidate)
        {
            _context.Candidates.Add(candidate);
            _context.SaveChanges();
        }

        public void CreateCandidates(IEnumerable<Models.Candidates.Candidate> candidates)
        {
            _context.Candidates.AddRange(candidates);
            _context.SaveChanges();
        }

        public Models.Candidates.Candidate GetCandidate(long id)
        {
            return _context.Candidates.Single(candidate => candidate.Id == id);
        }

        public IEnumerable<Models.Candidates.Candidate> GetCandidatesByOriginalIds(params long[] originalIds)
        {
            return _context.Candidates.Where(candidate => originalIds.Contains(candidate.OriginalId)).ToList();
        }
    }
}
