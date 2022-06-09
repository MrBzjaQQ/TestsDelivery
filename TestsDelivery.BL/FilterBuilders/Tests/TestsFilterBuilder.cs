using System;
using System.Linq.Expressions;
using TestsDelivery.DAL.Models.Test;
using TestsDelivery.DAL.Shared;

namespace TestsDelivery.BL.FilterBuilders.Tests
{
    public class TestsFilterBuilder : FilterBuilderBase<Test>, ITestsFilterBuilder
    {
        public ITestsFilterBuilder ByName(string searchTerm)
        {
            Expression<Func<Test, bool>> byName = x => x.Name.Contains(searchTerm);
            And(byName);
            return this;
        }
    }
}
