using AutoMapper;
using TestsPortal.BL.Services.Subjects;
using TestsPortal.DAL.Models.Questions;
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
            var subjects = _subjectsService.CreateSubjects(questions
                .Select(x => x.Subject)
                .Distinct());

            var questionsWithOptions = GetQuestionsOfType<QuestionWithOptionsBase>(
                questions.Where(
                    x => x.Type == QuestionType.MultipleChoice
                    || x.Type == QuestionType.SingleChoice));

            var dalQuestions = _mapper.Map<IEnumerable<Question>>(questions);
            _questionsRepository.CreateQuestions(dalQuestions);

            var options = questionsWithOptions.Aggregate(new List<Domain.Questions.AnswerOption>(), (accumulator, question) =>
            {
                return accumulator.Concat(question.AnswerOptions).ToList();
            });

            var dalOptions = _mapper.Map<IEnumerable<DAL.Models.Questions.AnswerOption>>(options);
            _answerOptionsRepository.CreateAnswerOptions(dalOptions);

            return MapQuestionsToDomainBase(dalQuestions, dalOptions);
        }

        private IEnumerable<TQuestionType> GetQuestionsOfType<TQuestionType>(IEnumerable<QuestionBase> questions)
            where TQuestionType : QuestionBase
        {
            foreach (var questionsItem in questions)
                yield return (TQuestionType)questionsItem;
        }

        private IEnumerable<QuestionBase> MapQuestionsToDomainBase(IEnumerable<Question> questions, IEnumerable<DAL.Models.Questions.AnswerOption> answerOptions)
        {
            foreach (var questionsItem in questions)
                yield return MapQuestion(questionsItem, answerOptions);
        }

        private QuestionBase MapQuestion(Question question, IEnumerable<DAL.Models.Questions.AnswerOption> answerOptions)
        {
            switch (question.Type)
            {
                case (short)QuestionType.Essay:
                    return _mapper.Map<EssayQuestion>(question);

                case (short)QuestionType.MultipleChoice:
                    return MapQuestionWithOptionsOfType<MultipleChoiceQuestion>(question, answerOptions);

                case (short)QuestionType.SingleChoice:
                    return MapQuestionWithOptionsOfType<SingleChoiceQuestion>(question, answerOptions);

                default:
                    throw new ArgumentException($"Unknown type of question ${question.Type}");
            }
        }

        private TQuestionType MapQuestionWithOptionsOfType<TQuestionType>(Question question, IEnumerable<DAL.Models.Questions.AnswerOption> answerOptions)
            where TQuestionType : QuestionWithOptionsBase
        {
            var domainQuestion = _mapper.Map<TQuestionType>(question);
            var options = answerOptions.Where(x => x.QuestionId == domainQuestion.Id);
            domainQuestion.AnswerOptions = _mapper.Map<IEnumerable<Domain.Questions.AnswerOption>>(options);
            return domainQuestion;
        }

        public IEnumerable<Domain.Questions.ShortQuestion> GetByTestId(long testId)
        {
            return _mapper.Map<IEnumerable<Domain.Questions.ShortQuestion>>(_questionsRepository.GetByTestId(testId));
        }

        public QuestionBase GetById(long questionId)
        {
            // TODO: implement ITypedQuestionService
            throw new NotImplementedException();
        }
    }
}
