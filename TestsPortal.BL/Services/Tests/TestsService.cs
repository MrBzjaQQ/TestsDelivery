using TestsPortal.Domain.Tests;
using TestsPortal.DAL.Repositories.Tests;
using AutoMapper;
using TestsPortal.BL.Services.Questions;

namespace TestsPortal.BL.Services.Tests
{
    public class TestsService : ITestsService
    {
        private ITestsRepository _testsRepository;
        private readonly IQuestionsService _questionsService;
        private readonly IMapper _mapper;

        public TestsService(
            ITestsRepository testsRepository,
            IQuestionsService questionsService,
            IMapper mapper)
        {
            _testsRepository = testsRepository;
            _questionsService = questionsService;
            _mapper = mapper;
        }

        public Test CreateTest(Test test)
        {
            var dalTest = _mapper.Map<DAL.Models.Tests.Test>(test);
            _testsRepository.Create(dalTest);
            var questions = _questionsService.CreateQuestions(test.Questions);
            var resultTest = _mapper.Map<Test>(dalTest);
            resultTest.Questions = questions;
            return resultTest;
        }
    }
}
