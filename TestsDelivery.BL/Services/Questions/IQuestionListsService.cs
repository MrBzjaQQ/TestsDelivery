using System.Collections.Generic;
using TestsDelivery.Domain.Lists;
using TestsDelivery.Domain.Questions;

namespace TestsDelivery.BL.Services.Questions
{
    public interface IQuestionListsService
    {
        IEnumerable<QuestionInListDto> GetList(ListFilter filter);
    }
}
