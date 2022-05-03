using System.Collections.Generic;
using TestsDelivery.UserModels.Candidate;
using TestsDelivery.UserModels.Lists;

namespace TestsDelivery.BL.Mediators.Candidate
{
    public interface ICandidateMediator
    {
        CandidateReadModel CreateCandidate(CandidateCreateModel model);

        CandidateReadModel GetCandidate(long id);

        void EditCandidate(CandidateEditModel model);

        IEnumerable<CandidateReadModel> GetList(ListModel model);
    }
}
