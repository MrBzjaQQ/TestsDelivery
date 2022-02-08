using AutoMapper;
using Moq;
using TestsDelivery.BL.Mappings;
using TestsDelivery.BL.Mediators.Questions.Essay;
using TestsDelivery.BL.Models.Questions.Essay;
using TestsDelivery.BL.Services.Questions.Essay;
using TestsDelivery.BL.Validators.Questions;
using TestsDelivery.Domain.Questions;
using Xunit;

namespace TestsDelivery.BL.UnitTests.Mediators.Questions.Essay
{
    public class EssayMediatorTests
    {
        private readonly Mock<IEssayValidator> _essayValidatorMock;
        private readonly Mock<IEssayService> _essayServiceMock;
        private readonly EssayMediator _mediator;
        private readonly Mapper _mapper;

        public EssayMediatorTests()
        {
            var mapperConfig = new MapperConfiguration(cfg => {
                cfg.AddProfile<QuestionMappings>();
                cfg.AddProfile<SubjectMappings>();
            });
            _essayValidatorMock = new Mock<IEssayValidator>();
            _essayServiceMock = new Mock<IEssayService>();
            _mapper = new Mapper(mapperConfig);
            
            _mediator = new EssayMediator(_essayServiceMock.Object, _essayValidatorMock.Object, _mapper);
        }

        [Fact]
        public void CreateQuestion_DependenciesCalled()
        {
            // Arrange
            var createModel = new EssayCreateModel
            {
                Name = "Name",
                SubjectId = 11,
                Text = "Text"
            };

            var questionToReturn = new EssayQuestion
            {
                Id = 1,
                Name = createModel.Name,
                Text = createModel.Text,
                Subject = new Domain.Subject.Subject { Id = createModel.SubjectId },
                Type = QuestionType.Essay
            };

            _essayServiceMock.Setup(x => x.CreateQuestion(It.IsAny<EssayQuestion>()))
                .Returns(questionToReturn);

            // Act
            var returnedQuestion = _mediator.CreateQuestion(createModel);

            // Assert
            Assert.Equal(questionToReturn.Name, createModel.Name);
            Assert.Equal(questionToReturn.Text, createModel.Text);
            Assert.Equal(questionToReturn.Name, createModel.Name);
            Assert.Equal(questionToReturn.Subject.Id, createModel.SubjectId);

            _essayValidatorMock.Verify(x => x.ValidateCreateModel(createModel), Times.Once);
            _essayServiceMock.Verify(x => x.CreateQuestion(It.IsAny<EssayQuestion>()), Times.Once);
        }

        [Fact]
        public void EditQuestion_DependenciesCalled()
        {
            // Arrange
            var editModel = new EssayEditModel
            {
                Id = 2,
                Name = "Name",
                SubjectId = 11,
                Text = "Text"
            };

            // Act
            _mediator.EditQuestion(editModel);

            // Assert
            _essayValidatorMock.Verify(x => x.ValidateEditModel(editModel), Times.Once);
            _essayServiceMock.Verify(x => x.EditQuestion(It.IsAny<EssayQuestion>()), Times.Once);
        }

        [Fact]
        public void DeleteQuestion_DependenciesCalled()
        {
            // Arrange
            const long deleteId = 2;

            // Act
            _mediator.DeleteQuestion(deleteId);

            // Assert
            _essayServiceMock.Verify(x => x.DeleteQuestion(deleteId), Times.Once);
        }

        [Fact]
        public void GetQuestion_DependenciesCalled()
        {
            // Arrange
            const long questionId = 2;

            var question = new EssayQuestion
            {
                Id = questionId,
                Name = "Name",
                Subject = new Domain.Subject.Subject { Id = 11 },
                Text = "Text",
                Type = QuestionType.Essay
            };

            _essayServiceMock.Setup(x => x.GetQuestion(questionId))
                .Returns(question);

            // Act
            var readModel = _mediator.GetQuestion(questionId);

            // Assert
            Assert.Equal(readModel.Id, question.Id);
            Assert.Equal(readModel.Name, question.Name);
            Assert.Equal(readModel.Subject.Id, question.Subject.Id);
            Assert.Equal(readModel.Text, question.Text);

            _essayServiceMock.Verify(x => x.GetQuestion(questionId), Times.Once);
        }
    }
}
