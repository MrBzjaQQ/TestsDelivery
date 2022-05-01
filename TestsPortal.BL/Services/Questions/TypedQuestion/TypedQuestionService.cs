using AutoMapper;

namespace TestsPortal.BL.Services.Questions.TypedQuestion
{
    public abstract class TypedQuestionService<TDomainModel> : ITypedQuestionService<TDomainModel>
    {
        public readonly IMapper Mapper;

        public TypedQuestionService(IMapper mapper)
        {
            Mapper = mapper;
        }

        public void PostAnswer(TDomainModel answer)
        {
            throw new NotImplementedException();
        }
    }
}
