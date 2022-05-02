using AutoMapper;
using TestsDelivery.UserModels.AnsweredQuestions.AnswerModels;
using TestsPortal.BL.Services.Questions.TypedQuestion;
using TestsPortal.Domain.AnsweredQuestions.Answers;

namespace TestsPortal.BL.Mediators.Questions
{
    public class MultipleChoiceMediator : QuestionMediatorBase<MultipleChoiceAnswerCreateModel, MultipleChoiceAnswer>, IMultipleChoiceMediator
    {
        public MultipleChoiceMediator(IMultipleChoiceService service, IMapper mapper)
            : base(service, mapper)
        {
        }
    }
}
