using TestsDelivery.UserModels.AnsweredQuestions.AnswerModels;
using TestsPortal.BL.Mediators.Questions;

namespace TestsPortal.Controllers.Questions
{
    public class EssayController : QuestionControllerBase<EssayAnswerModel>
    {
        public EssayController(IQuestionMediatorBase<EssayAnswerModel> mediator)
            : base(mediator)
        {
        }
    }
}
