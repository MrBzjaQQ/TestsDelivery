using TestsDelivery.UserModels.AnsweredQuestions.AnswerModels;
using TestsPortal.BL.Mediators.Questions;

namespace TestsPortal.Controllers.Questions
{
    public class SingleChoiceController : QuestionControllerBase<SingleChoiceAnswerCreateModel>
    {
        public SingleChoiceController(IQuestionMediatorBase<SingleChoiceAnswerCreateModel> mediator)
            : base(mediator)
        {
        }
    }
}
