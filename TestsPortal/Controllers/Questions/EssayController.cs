using TestsDelivery.UserModels.AnsweredQuestions.AnswerModels;
using TestsPortal.BL.Mediators.Questions;

namespace TestsPortal.Controllers.Questions
{
    public class EssayController : QuestionControllerBase<EssayAnswerCreateModel>
    {
        public EssayController(IQuestionMediatorBase<EssayAnswerCreateModel> mediator)
            : base(mediator)
        {
        }
    }
}
