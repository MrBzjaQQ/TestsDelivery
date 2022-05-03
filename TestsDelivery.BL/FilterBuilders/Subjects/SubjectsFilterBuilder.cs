using System;
using System.Linq.Expressions;
using TestsDelivery.DAL.Models.Subject;
using TestsDelivery.DAL.Shared;

namespace TestsDelivery.BL.FilterBuilders.Subjects
{
    public class SubjectsFilterBuilder : FilterBuilderBase<Subject>, ISubjectsFilterBuilder
    {
        public ISubjectsFilterBuilder ByName(string nameFilter)
        {
            Expression<Func<Subject, bool>> byName = x => x.Name.Contains(nameFilter);
            And(byName);
            return this;
        }
    }
}
