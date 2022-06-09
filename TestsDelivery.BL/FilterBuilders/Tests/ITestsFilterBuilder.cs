using TestsDelivery.DAL.Models.Test;
using TestsDelivery.DAL.Shared;

namespace TestsDelivery.BL.FilterBuilders.Tests
{
    public interface ITestsFilterBuilder : IFilterBuilderBase<Test>
    {
        ITestsFilterBuilder ByName(string searchTerm);
    }
}
