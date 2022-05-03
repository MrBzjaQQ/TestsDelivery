using TestsDelivery.DAL.Models.Subject;
using TestsDelivery.DAL.Shared;

namespace TestsDelivery.BL.FilterBuilders.Subjects
{
    public interface ISubjectsFilterBuilder : IFilterBuilderBase<Subject>
    {
        ISubjectsFilterBuilder ByName(string nameFilter);
    }
}
