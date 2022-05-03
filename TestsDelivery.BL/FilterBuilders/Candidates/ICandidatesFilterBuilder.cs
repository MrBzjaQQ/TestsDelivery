using TestsDelivery.DAL.Models.Candidate;
using TestsDelivery.DAL.Shared;

namespace TestsDelivery.BL.FilterBuilders.Candidates
{
    public interface ICandidatesFilterBuilder : IFilterBuilderBase<Candidate>
    {
        ICandidatesFilterBuilder ByName(string nameFilter);
    }
}
