using Moq;
using TestsDelivery.BL.Mediators.Questions.MultipleChoice;
using TestsDelivery.BL.Validators.Questions;
using TestsDelivery.BL.Services.Questions.MultipleChoice;
using AutoMapper;
using TestsDelivery.BL.Mappings;
using Xunit;
using TestsDelivery.UserModels.Questions.MultipleChoice;
using TestsDelivery.UserModels.AnswerOptions;
using TestsDelivery.Domain.Questions;
using System.Linq;

namespace TestsDelivery.BL.UnitTests.Mediators.Questions.MultipleChoice
{
    public class McqMediatorTests
    {
        private readonly Mock<McqModelValidator> _mcqValidatorMock;
        private readonly Mock<IMcqService> _mcqServiceMock;
        private readonly McqMediator _mediator;
        private readonly Mapper _mapper;

        public McqMediatorTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<QuestionMappings>();
                cfg.AddProfile<SubjectMappings>();
                cfg.AddProfile<AnswerOptionMappings>();
            });
            _mapper = new Mapper(mapperConfig);
            _mcqValidatorMock = new Mock<McqModelValidator>();
            _mcqServiceMock = new Mock<IMcqService>();
            _mediator = new McqMediator(_mcqServiceMock.Object, _mcqValidatorMock.Object, _mapper);
        }

        [Fact]
        public void CreateQuestion_DependenciesCalled()
        {
            // Arrange
            var createModelAnswerOpts = new[]
                {
                    new AnswerOptionCreateModel
                    {
                        Text = "Answer 1",
                        IsCorrect = true
                    },
                    new AnswerOptionCreateModel
                    {
                        Text = "Answer 2",
                        IsCorrect = false
                    }
                };

            var createModel = new McqCreateModel
            {
                Text = "Text",
                Name = "Name",
                SubjectId = 11,
                AnswerOptions = createModelAnswerOpts
            };

            var modelToReturn = new MultipleChoiceQuestion
            {
                Id = 1,
                Name = createModel.Name,
                Text = createModel.Text,
                Subject = new Domain.Subject.Subject { Id = createModel.SubjectId },
                AnswerOptions = new[]
                {
                    new AnswerOption
                    {
                        Id = 1,
                        Text = createModelAnswerOpts[0].Text,
                        IsCorrect = createModelAnswerOpts[0].IsCorrect
                    },
                    new AnswerOption
                    {
                        Id = 2,
                        Text = createModelAnswerOpts[1].Text,
                        IsCorrect = createModelAnswerOpts[1].IsCorrect
                    }
                }
            };

            _mcqValidatorMock.Setup(x => x.ValidateCreateModel(createModel));

            _mcqServiceMock.Setup(x => x.CreateQuestion(It.IsAny<MultipleChoiceQuestion>()))
                .Returns(modelToReturn);

            // Act
            var readModel = _mediator.CreateQuestion(createModel);

            // Assert
            Assert.Equal(modelToReturn.Text, readModel.Text);
            Assert.Equal(modelToReturn.Subject.Id, readModel.Subject.Id);
            Assert.Equal(modelToReturn.Name, readModel.Name);
            Assert.Equal(modelToReturn.AnswerOptions.Count(), readModel.AnswerOptions.Count());

            var expectedOptions = modelToReturn.AnswerOptions.ToList();
            var actualOptions = readModel.AnswerOptions.ToList();
            for (int i = 0; i < readModel.AnswerOptions.Count(); i++)
            {
                var expectedOption = expectedOptions[i];
                var actualOption = actualOptions[i];
                Assert.Equal(expectedOption.Text, actualOption.Text);
                Assert.Equal(expectedOption.IsCorrect, actualOption.IsCorrect);
            }

            _mcqValidatorMock.Verify(x => x.ValidateCreateModel(createModel), Times.Once);
            _mcqServiceMock.Verify(x => x.CreateQuestion(It.IsAny<MultipleChoiceQuestion>()), Times.Once);
        }

        [Fact]
        public void EditQuestion_DependenciesCalled()
        {
            // Arrange
            var editModelAnswerOpts = new[]
                {
                    new AnswerOptionEditModel
                    {
                        Id = 1,
                        Text = "Answer 1",
                        IsCorrect = true
                    },
                    new AnswerOptionEditModel
                    {
                        Id = 2,
                        Text = "Answer 2",
                        IsCorrect = false
                    }
                };

            var editModel = new McqEditModel
            {
                Id = 1,
                Text = "Text",
                Name = "Name",
                SubjectId = 11,
                AnswerOptions = editModelAnswerOpts
            };

            _mcqServiceMock.Setup(x => x.EditQuestion(It.IsAny<MultipleChoiceQuestion>()));
            _mcqValidatorMock.Setup(x => x.ValidateEditModel(editModel));

            // Act
            _mediator.EditQuestion(editModel);

            // Assert
            _mcqServiceMock.Verify(x => x.EditQuestion(It.IsAny<MultipleChoiceQuestion>()));
            _mcqValidatorMock.Setup(x => x.ValidateEditModel(editModel));
        }

        [Fact]
        public void GetQuestion_DependenciesCalled()
        {
            // Arrange
            var modelToReturn = new MultipleChoiceQuestion
            {
                Id = 1,
                Name = "Name",
                Text = "Text",
                Subject = new Domain.Subject.Subject { Id = 11 },
                AnswerOptions = new[]
                {
                    new AnswerOption
                    {
                        Id = 1,
                        Text = "Answer 1",
                        IsCorrect = true
                    },
                    new AnswerOption
                    {
                        Id = 2,
                        Text = "Answer 2",
                        IsCorrect = false
                    }
                }
            };

            _mcqServiceMock.Setup(x => x.GetQuestion(modelToReturn.Id)).Returns(modelToReturn);

            // Act
            var readModel = _mediator.GetQuestion(modelToReturn.Id);

            // Assert
            Assert.Equal(modelToReturn.Text, readModel.Text);
            Assert.Equal(modelToReturn.Subject.Id, readModel.Subject.Id);
            Assert.Equal(modelToReturn.Name, readModel.Name);
            Assert.Equal(modelToReturn.AnswerOptions.Count(), readModel.AnswerOptions.Count());

            var expectedOptions = modelToReturn.AnswerOptions.ToList();
            var actualOptions = readModel.AnswerOptions.ToList();
            for (int i = 0; i < readModel.AnswerOptions.Count(); i++)
            {
                var expectedOption = expectedOptions[i];
                var actualOption = actualOptions[i];
                Assert.Equal(expectedOption.Text, actualOption.Text);
                Assert.Equal(expectedOption.IsCorrect, actualOption.IsCorrect);
            }

            _mcqServiceMock.Verify(x => x.GetQuestion(modelToReturn.Id), Times.Once);
        }

        [Fact]
        public void DeleteQuestion_DependenciesCalled()
        {
            // Arrange
            const long deleteId = 11;

            _mcqServiceMock.Setup(x => x.DeleteQuestion(deleteId));

            // Act
            _mediator.DeleteQuestion(deleteId);

            // Assert
            _mcqServiceMock.Verify(x => x.DeleteQuestion(deleteId), Times.Once);
        }
    }
}
