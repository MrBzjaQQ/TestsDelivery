using Moq;
using AutoMapper;
using TestsDelivery.DAL.Repositories.Test;
using TestsDelivery.DAL.Repositories.QuestionInTests;
using TestsDelivery.DAL.Repositories.Questions;
using TestsDelivery.BL.Mappings;
using Xunit;
using TestsDelivery.BL.Services.Test;
using TestDomain = TestsDelivery.Domain.Test.Test;
using TestDAL = TestsDelivery.DAL.Models.Test.Test;
using TestsDelivery.Domain.Questions;
using TestsDelivery.DAL.Models.Test;
using System.Collections.Generic;
using System.Linq;
using TestsDelivery.DAL.Models.Questions;
using TestsDelivery.DAL.Repositories.AnswerOptions;

namespace TestsDelivery.BL.UnitTests.Services.Test
{
    public class TestServiceTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ITestRepository> _testRepoMock;
        private readonly Mock<IQuestionInTestRepository> _questionInTestRepoMock;
        private readonly Mock<IQuestionsRepository> _questionsRepoMock;
        private readonly Mock<IAnswerOptionsRepository> _answerOptionsRepositoryMock;
        private readonly TestService _service;

        public TestServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<TestMappings>();
                cfg.AddProfile<QuestionMappings>();
                cfg.AddProfile<SubjectMappings>();
                cfg.AddProfile<AnswerOptionMappings>();
            });
            _mapper = new Mapper(config);
            _testRepoMock = new Mock<ITestRepository>();
            _questionInTestRepoMock = new Mock<IQuestionInTestRepository>();
            _questionsRepoMock = new Mock<IQuestionsRepository>();
            _answerOptionsRepositoryMock = new Mock<IAnswerOptionsRepository>();
            _service = new TestService(
                _testRepoMock.Object,
                _questionInTestRepoMock.Object,
                _questionsRepoMock.Object,
                _answerOptionsRepositoryMock.Object,
                _mapper);
        }

        [Fact]
        public void CreateTest_DependenciesCalled()
        {
            // Arrange
            const long testId = 123;
            var test = new TestDomain()
            {
                Name = "Test Name",
                Questions = new[]
                {
                    new EssayQuestion()
                    {
                        Id = 2,
                    }
                },
            };

            var questions = test.Questions.Select(x => new Question
            {
                Id = x.Id,
            });

            _testRepoMock.Setup(x => x.Create(It.IsAny<TestDAL>()))
                .Callback<TestDAL>(x => x.Id = testId);
            _questionsRepoMock.Setup(x => x.GetQuestionsByTestId(testId))
                .Returns(questions);

            // Act
            var resultTest = _service.CreateTest(test);

            // Assert
            Assert.Equal(testId, resultTest.Id);
            Assert.Equal(test.Questions.Count(), resultTest.Questions.Count());
            Assert.Equal(test.Name, resultTest.Name);

            for (int i = 0; i < test.Questions.Count(); i++)
            {
                var question = test.Questions.ElementAt(i);
                var resultQuestion = resultTest.Questions.ElementAt(i);

                Assert.Equal(question.Id, resultQuestion.Id);
            }

            _questionInTestRepoMock.Verify(x => x.CreateQuestionsInTests(It.IsAny<List<QuestionInTest>>()), Times.Once);
            _testRepoMock.Verify(x => x.Create(It.IsAny<TestDAL>()), Times.Once);
            _questionsRepoMock.Verify(x => x.GetQuestionsByTestId(testId), Times.Once);
        }

        [Fact]
        public void GetTest_DependenciesCalled()
        {
            // Arrange
            const long testId = 123;
            var test = new TestDAL
            {
                Id = testId,
                Name = "Name"
            };

            var questions = new List<Question>
            {
                new Question
                {
                    Id = 1,
                }
            };

            _testRepoMock.Setup(x => x.GetById(testId)).Returns(test);
            _questionsRepoMock.Setup(x => x.GetQuestionsByTestId(testId)).Returns(questions);

            // Act
            _service.GetTest(testId);

            // Assert
            _testRepoMock.Verify(x => x.GetById(testId), Times.Once);
            _questionsRepoMock.Verify(x => x.GetQuestionsByTestId(testId), Times.Once);
        }

        [Fact]
        public void EditTest_DependenciesCalled()
        {
            // Arrange
            var test = new TestDomain()
            {
                Id = 123,
                Name = "Test Name",
                Questions = new[]
               {
                    new EssayQuestion()
                    {
                        Id = 2,
                    }
                },
            };

            // Act
            _service.EditTest(test);

            // Assert
            _testRepoMock.Verify(x => x.Update(It.IsAny<TestDAL>()), Times.Once);
            _questionInTestRepoMock.Verify(x => x.DeleteQuestionsForTest(test.Id), Times.Once);
            _questionInTestRepoMock.Verify(x => x.CreateQuestionsInTests(It.IsAny<IEnumerable<QuestionInTest>>()), Times.Once);
        }
    }
}
