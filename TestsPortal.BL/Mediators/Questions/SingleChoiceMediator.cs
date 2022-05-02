using AutoMapper;
using TestsDelivery.UserModels.AnsweredQuestions.AnswerModels;
using TestsPortal.BL.Services.Questions.TypedQuestion;
using TestsPortal.Domain.AnsweredQuestions.Answers;

namespace TestsPortal.BL.Mediators.Questions
{
    public class SingleChoiceMediator : QuestionMediatorBase<SingleChoiceAnswerCreateModel, SingleChoiceAnswer>, ISingleChoiceMediator
    {
        public SingleChoiceMediator(ISingleChoiceService service, IMapper mapper)
            : base(service, mapper)
        {
        }
    }
}
