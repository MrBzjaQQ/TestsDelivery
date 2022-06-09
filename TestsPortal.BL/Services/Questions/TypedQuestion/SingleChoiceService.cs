using AutoMapper;
using TestsPortal.DAL.Models.Questions;
using TestsPortal.DAL.Repositories.Answers;
using TestsPortal.Domain.AnsweredQuestions.Answers;

namespace TestsPortal.BL.Services.Questions.TypedQuestion
{
    public class SingleChoiceService : ISingleChoiceService
    {
        private IMapper _mapper;
        private readonly IChoiceAnswersRepository _answersRepository;

        public SingleChoiceService(IChoiceAnswersRepository answersRepository, IMapper mapper)
        {
            _mapper = mapper;
            _answersRepository = answersRepository;
        }

        public void PostAnswer(SingleChoiceAnswer answer)
        {
            var dalAnswer = _mapper.Map<ChoiceAnswer>(answer);
            _answersRepository.Create(dalAnswer);
        }
    }
}
