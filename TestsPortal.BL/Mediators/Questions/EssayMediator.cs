using AutoMapper;
using TestsDelivery.UserModels.AnsweredQuestions.AnswerModels;
using TestsPortal.BL.Services.Questions.TypedQuestion;
using TestsPortal.Domain.AnsweredQuestions.Answers;

namespace TestsPortal.BL.Mediators.Questions
{
    public class EssayMediator : QuestionMediatorBase<EssayAnswerModel, EssayAnswer>, IEssayMediator
    {
        public EssayMediator(IEssayService service, IMapper mapper)
            : base(service, mapper)
        {
        }
    }
}
