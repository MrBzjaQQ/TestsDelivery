using Xunit;
using Moq;
using AutoMapper;
using TestsDelivery.DAL.Repositories.Questions;
using TestsDelivery.BL.Services.Questions.Essay;
using TestsDelivery.BL.Mappings;
using TestsDelivery.Domain.Questions;
using TestsDelivery.DAL.Models.Questions;

namespace TestsDelivery.BL.UnitTests.Services.Questions.Essay
{
    public class EssayServiceTests
    {
        private readonly Mock<IQuestionsRepository> _questionRepositoryMock;
        private readonly IMapper _mapper;
        private readonly EssayService _service;

        public EssayServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<QuestionMappings>();
                cfg.AddProfile<SubjectMappings>();
            });
            _mapper = new Mapper(config);
            _questionRepositoryMock = new Mock<IQuestionsRepository>();
            _service = new EssayService(_questionRepositoryMock.Object, _mapper);
        }

        [Fact]
        public void CreateQuestion_DependenciesCalled()
        {
            // Arrange
            const long questionId = 1;
            var inputQuestion = new EssayQuestion
            {
                Name = "Name",
                Subject = new Domain.Subject.Subject()
                {
                    Id = 11,
                },
                Text = "Question text",
                Type = QuestionType.Essay
            };
            var createdQuestion = new Question
            {
                Id = questionId,
                Name = inputQuestion.Name,
                Text = inputQuestion.Text,
                Subject = new DAL.Models.Subject.Subject()
                {
                    Id = inputQuestion.Subject.Id,
                },
                Type = (short)QuestionType.Essay
            };

            _questionRepositoryMock.Setup(x => x.Create(It.IsAny<Question>()))
                .Callback<Question>(x => x.Id = questionId);
            _questionRepositoryMock.Setup(x => x.GetById(questionId)).Returns(createdQuestion);

            // Act
            var resultQuestion = _service.CreateQuestion(inputQuestion);

            // Assert
            Assert.Equal(questionId, resultQuestion.Id);
            Assert.Equal(inputQuestion.Name, resultQuestion.Name);
            Assert.Equal(inputQuestion.Subject.Id, resultQuestion.Subject.Id);
            Assert.Equal(inputQuestion.Text, resultQuestion.Text);
            Assert.Equal(inputQuestion.Type, resultQuestion.Type);

            _questionRepositoryMock.Verify(x => x.Create(It.IsAny<Question>()), Times.Once);
            _questionRepositoryMock.Verify(x => x.GetById(questionId), Times.Once);
        }

        [Fact]
        public void EditQuestion_DependenciesCalled()
        {
            // Arrange
            var question = new EssayQuestion();

            // Act
            _service.EditQuestion(question);

            // Assert
            _questionRepositoryMock.Verify(x => x.Update(It.IsAny<Question>()), Times.Once);
        }

        [Fact]
        public void GetQuestion_DependenciesCalled()
        {
            // Arrange
            const long questionId = 123;

            // Act
            _service.GetQuestion(questionId);

            // Assert
            _questionRepositoryMock.Verify(x => x.GetQuestion(questionId, (short)QuestionType.Essay), Times.Once);
        }

        [Fact]
        public void DeleteQuestion_DependenciesCalled()
        {
            // Arrange
            const long questionId = 123;

            // Act
            _service.DeleteQuestion(questionId);

            // Assert
            _questionRepositoryMock.Verify(x => x.DeleteQuestion(questionId, (short)QuestionType.Essay), Times.Once);
        }
    }
}
