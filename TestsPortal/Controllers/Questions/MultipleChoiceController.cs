using TestsDelivery.UserModels.AnsweredQuestions.AnswerModels;
using TestsPortal.BL.Mediators.Questions;

namespace TestsPortal.Controllers.Questions
{
    public class MultipleChoiceController : QuestionControllerBase<MultipleChoiceAnswerCreateModel>
    {
        public MultipleChoiceController(IQuestionMediatorBase<MultipleChoiceAnswerCreateModel> mediator)
            : base(mediator)
        {
        }
    }
}
