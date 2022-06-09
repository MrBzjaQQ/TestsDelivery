using System;
using System.Linq.Expressions;
using TestsDelivery.DAL.Models.Questions;
using TestsDelivery.DAL.Shared;

namespace TestsDelivery.BL.FilterBuilders.Questions
{
    public class QuestionsFilterBuilder : FilterBuilderBase<Question>, IQuestionsFilterBuilder
    {
        public IQuestionsFilterBuilder ByName(string searchCriteria)
        {
            Expression<Func<Question, bool>> byName = x => x.Name.Contains(searchCriteria);
            And(byName);
            return this;
        }

        public IQuestionsFilterBuilder BySubjectId(long subjectId)
        {
            Expression<Func<Question, bool>> bySubjectId = x => x.SubjectId == subjectId;
            And(bySubjectId);
            return this;
        }
    }
}
