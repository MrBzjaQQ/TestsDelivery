using AutoMapper;
using TestsPortal.BL.Services.Subjects;
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
        private readonly ISubjectsService _subjectsService;

        public QuestionsService(
            IQuestionsRepository questionsRepository,
            IAnswerOptionsRepository answerOptionsRepository,
            ISubjectsService subjectsService,
            IMapper mapper)
        {
            _questionsRepository = questionsRepository;
            _answerOptionsRepository = answerOptionsRepository;
            _subjectsService = subjectsService;
            _mapper = mapper;
        }

        public IEnumerable<QuestionBase> CreateQuestions(IEnumerable<QuestionBase> questions)
        {
            // TODO: use conditional mappings
            // TODO: create subjects
            throw new NotImplementedException();
        }
    }
}
