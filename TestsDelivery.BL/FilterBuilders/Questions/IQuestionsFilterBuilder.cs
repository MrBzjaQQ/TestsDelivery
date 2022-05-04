using TestsDelivery.DAL.Models.Questions;
using TestsDelivery.DAL.Shared;

namespace TestsDelivery.BL.FilterBuilders.Questions
{
    public interface IQuestionsFilterBuilder : IFilterBuilderBase<Question>
    {
        IQuestionsFilterBuilder ByName(string searchCriteria);
    }
}
