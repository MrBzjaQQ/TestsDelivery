using AutoMapper;
using Moq;
using System.Linq;
using TestsDelivery.BL.Mappings;
using TestsDelivery.BL.Mediators.Test;
using TestsDelivery.UserModels.Test;
using TestsDelivery.BL.Services.Test;
using TestsDelivery.Domain.Questions;
using Xunit;

namespace TestsDelivery.BL.UnitTests.Mediators
{
    public class TestMediatorTests
    {
        private readonly Mapper _mapper;
        private readonly TestMediator _mediator;
        private readonly Mock<ITestService> _testServiceMock;

        public TestMediatorTests()
        {
            var mapperConfig = new MapperConfiguration(cfg => {
                cfg.AddProfile<TestMappings>();
                cfg.AddProfile<QuestionMappings>();
                cfg.AddProfile<SubjectMappings>();
            });
            _mapper = new Mapper(mapperConfig);
            _testServiceMock = new Mock<ITestService>();
            _mediator = new TestMediator(_testServiceMock.Object, _mapper);
        }

        [Fact]
        public void CreateTest_DependenciesCalled()
        {
            // Arrange
            var createModel = new TestCreateModel
            {
                Name = "Name",
                QuestionIds = new long[] { 1, 2, 3 },
            };

            var serviceModel = new Domain.Test.Test
            {
                Id = 1,
                Name = createModel.Name,
                Questions = createModel.QuestionIds.Select(id => new EssayQuestion { Id = id })
            };

            _testServiceMock.Setup(x => x.CreateTest(It.IsAny<Domain.Test.Test>())).Returns(serviceModel);

            // Act
            var readModel = _mediator.CreateTest(createModel);

            // Assert
            var questionIds = createModel.QuestionIds.ToList();
            var questions = readModel.Questions.ToList();

            Assert.Equal(createModel.Name, readModel.Name);
            Assert.Equal(questionIds.Count, questions.Count);
            for (int i = 0; i < questions.Count; i++)
            {
                var question = questions[i];
                var questionId = questionIds[i];

                Assert.Equal(questionId, question.Id);
            }

            _testServiceMock.Verify(x => x.CreateTest(It.IsAny<Domain.Test.Test>()), Times.Once);
        }

        [Fact]
        public void EditTest_DependenciesCalled()
        {
            // Arrange
            var editModel = new TestEditModel
            {
                Name = "Name",
                QuestionIds = new long[] { 1, 2, 3 },
            };

            _testServiceMock.Setup(x => x.EditTest(It.IsAny<Domain.Test.Test>()));

            // Act
            _mediator.EditTest(editModel);

            // Assert
            _testServiceMock.Verify(x => x.EditTest(It.IsAny<Domain.Test.Test>()), Times.Once);
        }

        [Fact]
        public void GetTest_DependenciesCalled()
        {
            // Arrange
            const long id = 1;

            var serviceModel = new Domain.Test.Test
            {
                Id = id,
                Name = "Name"
            };

            _testServiceMock.Setup(x => x.GetTest(id)).Returns(serviceModel);

            // Act
            var readModel = _mediator.GetTest(id);

            // Assert
            Assert.Equal(serviceModel.Name, readModel.Name);
            Assert.Equal(id, readModel.Id);

            _testServiceMock.Verify(x => x.GetTest(id), Times.Once);
        }
    }
}
