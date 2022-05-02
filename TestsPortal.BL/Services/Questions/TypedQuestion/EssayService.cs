using AutoMapper;
using TestsPortal.DAL.Models.Questions;
using TestsPortal.DAL.Repositories.Answers;
using TestsPortal.Domain.AnsweredQuestions.Answers;

namespace TestsPortal.BL.Services.Questions.TypedQuestion
{
    public class EssayService : IEssayService
    {
        private readonly IMapper _mapper;
        private readonly ITextAnswersRepository _answersRepository;

        public EssayService(ITextAnswersRepository answersRepository, IMapper mapper)
        {
            _mapper = mapper;
            _answersRepository = answersRepository;
        }

        public void PostAnswer(EssayAnswer answer)
        {
            var dalAnswer = _mapper.Map<TextAnswer>(answer);
            _answersRepository.Create(dalAnswer);
        }
    }
}
