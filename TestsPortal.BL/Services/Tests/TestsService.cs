using TestsPortal.Domain.Tests;
using TestsPortal.DAL.Repositories.Tests;
using AutoMapper;
using TestsPortal.BL.Services.Questions;
using QuestionInTest = TestsPortal.DAL.Models.Tests.QuestionInTest;
using TestsPortal.DAL.Repositories.QuestionsInTest;

namespace TestsPortal.BL.Services.Tests
{
    public class TestsService : ITestsService
    {
        private ITestsRepository _testsRepository;
        private readonly IQuestionsService _questionsService;
        private readonly IQuestionsInTestRepository _questionsInTestRepository;
        private readonly IMapper _mapper;

        public TestsService(
            ITestsRepository testsRepository,
            IQuestionsInTestRepository questionsInTestRepository,
            IQuestionsService questionsService,
            
            IMapper mapper)
        {
            _testsRepository = testsRepository;
            _questionsService = questionsService;
            _questionsInTestRepository = questionsInTestRepository;
            _mapper = mapper;
        }

        public Test CreateTest(Test test)
        {
            var dalTest = _mapper.Map<DAL.Models.Tests.Test>(test);
            _testsRepository.Create(dalTest);
            var questions = _questionsService.CreateQuestions(test.Questions);
            var questionsInTest = MapQuestionsToQuestionsInTest(questions, dalTest.Id);
            _questionsInTestRepository.Create(questionsInTest);
            var resultTest = _mapper.Map<Test>(dalTest);
            resultTest.Questions = questions;
            return resultTest;
        }

        private List<QuestionInTest> MapQuestionsToQuestionsInTest(IEnumerable<Domain.Questions.QuestionBase> questions, long testId)
        {
            List<QuestionInTest> questionsInTest = new();
            foreach (var testQuestion in questions)
                questionsInTest.Add(new QuestionInTest
                {
                    QuestionId = testQuestion.Id,
                    TestId = testId
                });

            return questionsInTest;
        }
    }
}
