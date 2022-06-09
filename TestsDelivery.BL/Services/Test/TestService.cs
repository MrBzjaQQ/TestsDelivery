using System.Collections.Generic;
using AutoMapper;
using TestsDelivery.DAL.Models.Test;
using TestsDelivery.DAL.Repositories.QuestionInTests;
using TestsDelivery.DAL.Repositories.Questions;
using TestsDelivery.DAL.Repositories.Test;
using TestsDelivery.Domain.Questions;
using TestDomain = TestsDelivery.Domain.Test.Test;
using TestData = TestsDelivery.DAL.Models.Test.Test;
using TestsDelivery.DAL.Repositories.AnswerOptions;
using System.Linq;
using TestsDelivery.DAL.Exceptions.Questions;
using TestsDelivery.Domain.Lists;
using TestsDelivery.BL.FilterBuilders.Tests;
using TestsDelivery.Domain.Test;

namespace TestsDelivery.BL.Services.Test
{
    public class TestService : ITestService
    {
        private readonly IMapper _mapper;
        private readonly ITestRepository _testRepository;
        private readonly IQuestionInTestRepository _questionInTestRepository;
        private readonly IQuestionsRepository _questionsRepository;
        private readonly IAnswerOptionsRepository _answerOptionsRepository;

        public TestService(
            ITestRepository testRepository,
            IQuestionInTestRepository questionInTestRepository,
            IQuestionsRepository questionsRepository,
            IAnswerOptionsRepository answerOptionsRepository,
            IMapper mapper)
        {
            _testRepository = testRepository;
            _questionInTestRepository = questionInTestRepository;
            _questionsRepository = questionsRepository;
            _answerOptionsRepository = answerOptionsRepository;
            _mapper = mapper;
        }

        public TestDomain CreateTest(TestDomain test)
        {
            var testData = _mapper.Map<TestData>(test);
            _testRepository.Create(testData);

            var questionsInTest = MapQuestionsToQuestionsInTest(test.Questions, testData.Id);
            _questionInTestRepository.Create(questionsInTest);

            return MapTestToDomainAndGetQuestions(testData);
        }

        public TestDomain GetTest(long id)
        {
            var testData = _testRepository.GetById(id);

            return MapTestToDomainAndGetQuestions(testData);
        }

        public TestDomain GetFullTest(long id)
        {
            var testData = _testRepository.GetById(id);

            return MapFullTestToDomain(testData);
        }

        public void EditTest(TestDomain test)
        {
            var testData = _mapper.Map<TestData>(test);
            _testRepository.Update(testData);

            _questionInTestRepository.DeleteQuestionsForTest(testData.Id);

            var questionsInTest = MapQuestionsToQuestionsInTest(test.Questions, test.Id);
            _questionInTestRepository.Create(questionsInTest);
        }

        public TestsList GetList(ListFilter filter)
        {
            var filterBuilder = new TestsFilterBuilder();

            if (filter.SearchText != null)
                filterBuilder.ByName(filter.SearchText);

            if (filter.Take.HasValue)
                filterBuilder.Take(filter.Take.Value);

            if (filter.Skip.HasValue)
                filterBuilder.Skip(filter.Skip.Value);

            var genericFilter = filterBuilder.Build();

            return new TestsList
            {
                Tests = _testRepository.GetWithProjection<TestInListDto>(genericFilter),
                TotalCount = _testRepository.Count(genericFilter)
            };
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

        private TestDomain MapFullTestToDomain(TestData test)
        {
            var testDomain = _mapper.Map<TestDomain>(test);
            testDomain.Questions = GetFullQuestions(test.Id);

            return testDomain;
        }

        private IEnumerable<QuestionBase> GetFullQuestions(long testId)
        {
            var questions = _questionsRepository.GetQuestionsByTestId(testId);
            var answerOptions = _answerOptionsRepository.GetAnswerOptionsForQuestionIds(questions.Select(x => x.Id)).ToLookup(x => x.QuestionId);

            foreach (var question in questions)
            {
                switch (question.Type)
                {
                    case (short)QuestionType.SingleChoice:
                        {
                            var domainQuestion = _mapper.Map<SingleChoiceQuestion>(question);
                            domainQuestion.AnswerOptions = _mapper.Map<IEnumerable<AnswerOption>>(answerOptions[domainQuestion.Id]);
                            yield return domainQuestion;
                            break;
                        }
                    case (short)QuestionType.MultipleChoice:
                        {
                            var domainQuestion = _mapper.Map<MultipleChoiceQuestion>(question);
                            domainQuestion.AnswerOptions = _mapper.Map<IEnumerable<AnswerOption>>(answerOptions[domainQuestion.Id]);
                            yield return domainQuestion;
                            break;
                        }
                    case (short)QuestionType.Essay:
                        {
                            yield return _mapper.Map<EssayQuestion>(question);
                            break;
                        }
                    default:
                        {
                            throw new QuestionTypeDoesNotExistsException(question.Type);
                        }
                }
            }
        }
    }
}
