namespace TestsPortal.DAL.Repositories.Candidate
{
    public interface ICandidatesRepository
    {
        Models.Candidates.Candidate GetCandidate(long id);

        IEnumerable<Models.Candidates.Candidate> GetCandidatesByOriginalIds(params long[] originalIds);

        void CreateCandidate(Models.Candidates.Candidate candidate);

        void CreateCandidates(IEnumerable<Models.Candidates.Candidate> candidates);
    }
}
