using AutoMapper;
using TestsPortal.BL.Services.Questions.TypedQuestion;

namespace TestsPortal.BL.Mediators.Questions
{
    public abstract class QuestionMediatorBase<TAnswerModel, TDomainModel> : IQuestionMediatorBase<TAnswerModel>
    {
        protected readonly ITypedQuestionService<TDomainModel> Service;
        protected readonly IMapper Mapper;

        public QuestionMediatorBase(ITypedQuestionService<TDomainModel> service, IMapper mapper)
        {
            Service = service;
            Mapper = mapper;
        }

        public void PostAnswer(TAnswerModel answer)
        {
            throw new NotImplementedException();
        }
    }
}
