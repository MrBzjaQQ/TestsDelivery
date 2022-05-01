using TestsDelivery.UserModels.AnsweredQuestions.AnswerModels;
using TestsPortal.BL.Mediators.Questions;

namespace TestsPortal.Controllers.Questions
{
    public class MultipleChoiceController : QuestionControllerBase<MultipleChoiceAnswerModel>
    {
        public MultipleChoiceController(IQuestionMediatorBase<MultipleChoiceAnswerModel> mediator)
            : base(mediator)
        {
        }
    }
}
