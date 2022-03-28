using AutoMapper;
using TestsPortal.DAL.Repositories.AnswerOptions;
using TestsPortal.DAL.Repositories.Questions;
using TestsPortal.Domain.Questions;

namespace TestsPortal.BL.Services.Questions
{
    public class QuestionsService : IQuestionsService
    {
        private readonly IMapper _mapper;
        private readonly IQuestionsRepository _questionsRepository;
        private readonly IAnswerOptionsRepository _answerOptionsRepository;

        public QuestionsService(
            IQuestionsRepository questionsRepository,
            IAnswerOptionsRepository answerOptionsRepository,
            IMapper mapper)
        {
            _questionsRepository = questionsRepository;
            _answerOptionsRepository = answerOptionsRepository;
            _mapper = mapper;
        }

        public IEnumerable<QuestionBase> CreateQuestions(IEnumerable<QuestionBase> questions)
        {
            // TODO: use conditional mappings
            throw new NotImplementedException();
        }
    }
}
