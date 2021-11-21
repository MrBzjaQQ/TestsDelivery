using System.Collections.Generic;
using AutoMapper;
using TestsDelivery.DAL.Models.Test;
using TestsDelivery.DAL.Repositories.QuestionInTests;
using TestsDelivery.DAL.Repositories.Questions;
using TestsDelivery.DAL.Repositories.Test;
using TestsDelivery.Domain.Questions;
using TestDomain = TestsDelivery.Domain.Test.Test;
using TestData = TestsDelivery.DAL.Models.Test.Test;

namespace TestsDelivery.BL.Services.Test
{
    public class TestService : ITestService
    {
        private readonly IMapper _mapper;
        private readonly ITestRepository _testRepository;
        private readonly IQuestionInTestRepository _questionInTestRepository;
        private readonly IQuestionsRepository _questionsRepository;

        public TestService(
            ITestRepository testRepository,
            IQuestionInTestRepository questionInTestRepository,
            IQuestionsRepository questionsRepository,
            IMapper mapper)
        {
            _testRepository = testRepository;
            _questionInTestRepository = questionInTestRepository;
            _questionsRepository = questionsRepository;
            _mapper = mapper;
        }

        public TestDomain CreateTest(TestDomain test)
        {
            var testData = _mapper.Map<TestData>(test);
            _testRepository.CreateTest(testData);

            var questionsInTest = MapQuestionsToQuestionsInTest(test.Questions, testData.Id);
            _questionInTestRepository.CreateQuestionInTests(questionsInTest);

            return MapTestToDomainAndGetQuestions(testData);
        }

        public TestDomain GetTest(long id)
        {
            var testData = _testRepository.GetTest(id);

            return MapTestToDomainAndGetQuestions(testData);
        }

        public void EditTest(TestDomain test)
        {
            var testData = _mapper.Map<TestData>(test);
            _testRepository.EditTest(testData);

            _questionInTestRepository.DeleteQuestionsForTest(testData.Id);

            var questionsInTest = MapQuestionsToQuestionsInTest(test.Questions, test.Id);
            _questionInTestRepository.CreateQuestionInTests(questionsInTest);
        }

        private List<QuestionInTest> MapQuestionsToQuestionsInTest(IEnumerable<QuestionBase> questions, long testId)
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

        private TestDomain MapTestToDomainAndGetQuestions(TestData test)
        {
            var testDomain = _mapper.Map<TestDomain>(test);
            testDomain.Questions = _mapper.Map<IEnumerable<QuestionBase>>(_questionsRepository.GetQuestionsByTestId(test.Id));
            return testDomain;
        }
    }
}
