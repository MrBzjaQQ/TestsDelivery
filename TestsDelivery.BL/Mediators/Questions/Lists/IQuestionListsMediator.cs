using System.Collections.Generic;
using TestsDelivery.UserModels.ListFilters;
using TestsDelivery.UserModels.Questions;
using TestsDelivery.UserModels.Subject;

namespace TestsDelivery.BL.Mediators.Questions.Lists
{
    public interface IQuestionListsMediator
    {
        IEnumerable<SubjectWithQuestionsModel> GetQuestionsInSubjects(ListFilterModel model);

        QuestionsListModel GetQuestionsForSubject(QuestionsInSubjectListFilterModel model);
    }
}
