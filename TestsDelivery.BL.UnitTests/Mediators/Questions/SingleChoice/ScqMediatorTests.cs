using AutoMapper;
using Moq;
using System.Linq;
using TestsDelivery.BL.Mappings;
using TestsDelivery.BL.Mediators.Questions.SingleChoice;
using TestsDelivery.UserModels.AnswerOptions;
using TestsDelivery.UserModels.Questions.SingleChoice;
using TestsDelivery.BL.Services.Questions.SingleChoice;
using TestsDelivery.UserModels.Validators.Questions;
using TestsDelivery.Domain.Questions;
using Xunit;

namespace TestsDelivery.BL.UnitTests.Mediators.Questions.SingleChoice
{
    public class ScqMediatorTests
    {
        private readonly Mock<ScqModelValidator> _scqValidatorMock;
        private readonly Mock<IScqService> _scqServiceMock;
        private readonly ScqMediator _mediator;
        private readonly Mapper _mapper;

        public ScqMediatorTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<QuestionMappings>();
                cfg.AddProfile<SubjectMappings>();
                cfg.AddProfile<AnswerOptionMappings>();
            });
            _mapper = new Mapper(mapperConfig);
            _scqValidatorMock = new Mock<ScqModelValidator>();
            _scqServiceMock = new Mock<IScqService>();
            _mediator = new ScqMediator(_scqServiceMock.Object, _scqValidatorMock.Object, _mapper);
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

            var createModel = new ScqCreateModel
            {
                Text = "Text",
                Name = "Name",
                SubjectId = 11,
                AnswerOptions = createModelAnswerOpts
            };

            var modelToReturn = new SingleChoiceQuestion
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

            _scqValidatorMock.Setup(x => x.ValidateCreateModel(createModel));

            _scqServiceMock.Setup(x => x.CreateQuestion(It.IsAny<SingleChoiceQuestion>()))
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

            _scqValidatorMock.Verify(x => x.ValidateCreateModel(createModel), Times.Once);
            _scqServiceMock.Verify(x => x.CreateQuestion(It.IsAny<SingleChoiceQuestion>()), Times.Once);
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

            var editModel = new ScqEditModel
            {
                Id = 1,
                Text = "Text",
                Name = "Name",
                SubjectId = 11,
                AnswerOptions = editModelAnswerOpts
            };

            _scqServiceMock.Setup(x => x.EditQuestion(It.IsAny<SingleChoiceQuestion>()));
            _scqValidatorMock.Setup(x => x.ValidateEditModel(editModel));

            // Act
            _mediator.EditQuestion(editModel);

            // Assert
            _scqServiceMock.Verify(x => x.EditQuestion(It.IsAny<SingleChoiceQuestion>()));
            _scqValidatorMock.Setup(x => x.ValidateEditModel(editModel));
        }

        [Fact]
        public void GetQuestion_DependenciesCalled()
        {
            // Arrange
            var modelToReturn = new SingleChoiceQuestion
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

            _scqServiceMock.Setup(x => x.GetQuestion(modelToReturn.Id)).Returns(modelToReturn);

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

            _scqServiceMock.Verify(x => x.GetQuestion(modelToReturn.Id), Times.Once);
        }

        [Fact]
        public void DeleteQuestion_DependenciesCalled()
        {
            // Arrange
            const long deleteId = 11;

            _scqServiceMock.Setup(x => x.DeleteQuestion(deleteId));

            // Act
            _mediator.DeleteQuestion(deleteId);

            // Assert
            _scqServiceMock.Verify(x => x.DeleteQuestion(deleteId), Times.Once);
        }
    }
}
