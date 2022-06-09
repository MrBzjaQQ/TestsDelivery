using Xunit;
using AutoMapper;
using Moq;
using TestsDelivery.DAL.Repositories.Questions;
using TestsDelivery.BL.Services.Questions.MultipleChoice;
using TestsDelivery.BL.Mappings;
using TestsDelivery.DAL.Repositories.AnswerOptions;
using TestsDelivery.Domain.Questions;
using TestsDelivery.DAL.Models.Questions;
using System.Collections.Generic;

namespace TestsDelivery.BL.UnitTests.Services.Questions.MultipleChoice
{
    public class McqServiceTests
    {
        private readonly Mock<IQuestionsRepository> _questionRepositoryMock;
        private readonly Mock<IAnswerOptionsRepository> _answerOptionsRepoMock;
        private readonly IMapper _mapper;
        private readonly McqService _service;

        public McqServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<QuestionMappings>();
                cfg.AddProfile<SubjectMappings>();
                cfg.AddProfile<AnswerOptionMappings>();
            });
            _mapper = new Mapper(config);
            _questionRepositoryMock = new Mock<IQuestionsRepository>();
            _answerOptionsRepoMock = new Mock<IAnswerOptionsRepository>();
            _service = new McqService(_questionRepositoryMock.Object, _answerOptionsRepoMock.Object, _mapper);
        }

        [Fact]
        public void CreateQuestion_DependenciesCalled()
        {
            // Arrange
            const long questionId = 213;
            var inputQuestion = new MultipleChoiceQuestion()
            {
                Name = "Name",
                Text = "Test",
                Type = QuestionType.MultipleChoice,
                Subject = new Domain.Subject.Subject { Id = 12 },
                AnswerOptions = new Domain.Questions.AnswerOption[]
                {
                    new Domain.Questions.AnswerOption
                    {
                        Text = "Option 1",
                        IsCorrect = true
                    },
                    new Domain.Questions.AnswerOption
                    {
                        Text = "Option 2",
                        IsCorrect = true
                    },
                    new Domain.Questions.AnswerOption
                    {
                        Text = "Option 3",
                        IsCorrect = false
                    },
                }
            };

            var createdQuestion = new Question()
            {
                Id = questionId,
                Name = inputQuestion.Name,
                Text = inputQuestion.Text,
                Type = (short)inputQuestion.Type,
                Subject = new DAL.Models.Subject.Subject { Id = inputQuestion.Subject.Id }
            };

            _questionRepositoryMock.Setup(x => x.Create(It.IsAny<Question>()))
                .Callback<Question>(x => x.Id = questionId);
            _questionRepositoryMock.Setup(x => x.GetById(questionId)).Returns(createdQuestion);
            _answerOptionsRepoMock.Setup(x => x.CreateAnswerOptions(It.IsAny<IEnumerable<DAL.Models.Questions.AnswerOption>>()));

            // Act
            var resultQuestion = _service.CreateQuestion(inputQuestion);

            // Assert
            Assert.Equal(questionId, resultQuestion.Id);
            Assert.Equal(createdQuestion.Name, resultQuestion.Name);
            Assert.Equal(createdQuestion.Text, resultQuestion.Text);
            Assert.Equal(createdQuestion.Subject.Id, resultQuestion.Subject.Id);
            Assert.Equal(createdQuestion.Type, (short)resultQuestion.Type);

            _questionRepositoryMock.Verify(x => x.Create(It.IsAny<Question>()), Times.Once);
            _questionRepositoryMock.Verify(x => x.GetById(questionId), Times.Once);
            _answerOptionsRepoMock.Verify(x => x.CreateAnswerOptions(It.IsAny<IEnumerable<DAL.Models.Questions.AnswerOption>>()), Times.Once);
        }

        [Fact]
        public void DeleteQuestion_DependenciesCalled()
        {
            // Arrange
            const long questionId = 132;

            // Act
            _service.DeleteQuestion(questionId);

            // Assert
            _answerOptionsRepoMock.Verify(x => x.DeleteAnswerOptionsByQuestionId(questionId), Times.Once);
            _questionRepositoryMock.Verify(x => x.DeleteQuestion(questionId, (short)QuestionType.MultipleChoice), Times.Once);
        }

        [Fact]
        public void EditQuestion_DependenciesCalled()
        {
            // Arrange
            var inputQuestion = new MultipleChoiceQuestion()
            {
                Id = 123,
                Name = "Name",
                Text = "Test",
                Type = QuestionType.MultipleChoice,
                Subject = new Domain.Subject.Subject { Id = 12 },
                AnswerOptions = new Domain.Questions.AnswerOption[]
                {
                    new Domain.Questions.AnswerOption
                    {
                        Text = "Option 1",
                        IsCorrect = true
                    },
                    new Domain.Questions.AnswerOption
                    {
                        Text = "Option 2",
                        IsCorrect = true
                    },
                    new Domain.Questions.AnswerOption
                    {
                        Text = "Option 3",
                        IsCorrect = false
                    },
                }
            };

            // Act
            _service.EditQuestion(inputQuestion);

            // Assert
            _answerOptionsRepoMock.Verify(x => x.EditAnswerOptions(It.IsAny<IEnumerable<DAL.Models.Questions.AnswerOption>>()), Times.Once);
            _answerOptionsRepoMock.Verify(x => x.CreateAnswerOptions(It.IsAny<IEnumerable<DAL.Models.Questions.AnswerOption>>()), Times.Once);
            _answerOptionsRepoMock.Verify(x => x.GetAnswerOptionsForQuestion(inputQuestion.Id), Times.Once);
            _answerOptionsRepoMock.Verify(x => x.DeleteAnswerOptions(It.IsAny<IEnumerable<long>>()), Times.Once);
            _questionRepositoryMock.Verify(x => x.Update(It.IsAny<Question>()), Times.Once);
        }

        [Fact]
        public void GetQuestion_DependenciesCalled()
        {
            // Arrange
            const long questionId = 123;

            // Act
            _service.DeleteQuestion(questionId);

            // Assert
            _answerOptionsRepoMock.Verify(x => x.DeleteAnswerOptionsByQuestionId(questionId), Times.Once);
            _questionRepositoryMock.Verify(x => x.DeleteQuestion(questionId, (short)QuestionType.MultipleChoice), Times.Once);
        }
    }
}
