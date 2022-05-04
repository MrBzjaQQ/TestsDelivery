using System.Collections.Generic;
using TestsDelivery.UserModels.Lists;
using TestsDelivery.UserModels.Subject;

namespace TestsDelivery.BL.Mediators.Questions.Lists
{
    public interface IQuestionListsMediator
    {
        IEnumerable<SubjectWithQuestionsModel> GetQuestionsInSubjects(ListModel model);
    }
}
