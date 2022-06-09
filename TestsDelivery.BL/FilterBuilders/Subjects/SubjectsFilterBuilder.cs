using System;
using System.Linq.Expressions;
using TestsDelivery.DAL.Models.Subject;
using TestsDelivery.DAL.Shared;

namespace TestsDelivery.BL.FilterBuilders.Subjects
{
    public class SubjectsFilterBuilder : FilterBuilderBase<Subject>, ISubjectsFilterBuilder
    {
        public ISubjectsFilterBuilder ByIsRetired(bool isRetired)
        {
            Expression<Func<Subject, bool>> byIsRetired = x => x.Retired == isRetired;
            And(byIsRetired);
            return this;
        }

        public ISubjectsFilterBuilder ByName(string nameFilter)
        {
            Expression<Func<Subject, bool>> byName = x => x.Name.Contains(nameFilter);
            And(byName);
            return this;
        }
    }
}
