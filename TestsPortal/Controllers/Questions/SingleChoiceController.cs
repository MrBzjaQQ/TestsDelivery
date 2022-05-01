using TestsDelivery.UserModels.AnsweredQuestions.AnswerModels;
using TestsPortal.BL.Mediators.Questions;

namespace TestsPortal.Controllers.Questions
{
    public class SingleChoiceController : QuestionControllerBase<SingleChoiceAnswerModel>
    {
        public SingleChoiceController(IQuestionMediatorBase<SingleChoiceAnswerModel> mediator)
            : base(mediator)
        {
        }
    }
}
