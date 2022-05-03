using System;
using System.Linq.Expressions;
using TestsDelivery.DAL.Models.Candidate;
using TestsDelivery.DAL.Shared;

namespace TestsDelivery.BL.FilterBuilders.Candidates
{
    public class CandidatesFilterBuilder : FilterBuilderBase<Candidate>, ICandidatesFilterBuilder
    {
        public ICandidatesFilterBuilder ByName(string nameFilter)
        {
            Expression<Func<Candidate, bool>> byName = x => x.FirstName.Contains(nameFilter)
                || x.LastName.Contains(nameFilter)
                || (x.FirstName + x.LastName).Contains(nameFilter);
            And(byName);
            return this;
        }
    }
}
