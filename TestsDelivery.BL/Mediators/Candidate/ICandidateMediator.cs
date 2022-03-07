using TestsDelivery.UserModels.Candidate;

namespace TestsDelivery.BL.Mediators.Candidate
{
    public interface ICandidateMediator
    {
        CandidateReadModel CreateCandidate(CandidateCreateModel model);

        CandidateReadModel GetCandidate(long id);

        void EditCandidate(CandidateEditModel model);
    }
}
