namespace TestsDelivery.DAL.Repositories.Candidate
{
    public interface ICandidateRepository
    {
        Models.Candidate.Candidate GetCandidate(long id);

        void CreateCandidate(Models.Candidate.Candidate candidate);

        void EditCandidate(Models.Candidate.Candidate candidate);
    }
}
