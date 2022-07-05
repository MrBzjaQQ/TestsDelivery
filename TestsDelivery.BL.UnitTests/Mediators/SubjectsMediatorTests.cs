using AutoMapper;
using Moq;
using TestsDelivery.BL.Mediators.Subjects;
using TestsDelivery.BL.Services.Subjects;
using TestsDelivery.BL.Mappings;
using Xunit;
using TestsDelivery.UserModels.Subject;
using TestsDelivery.Domain.Subject;

namespace TestsDelivery.BL.UnitTests.Mediators
{
    public class SubjectsMediatorTests
    {
        private readonly Mock<ISubjectsService> _serviceMock;
        private readonly IMapper _mapper;
        private readonly SubjectsMediator _mediator;
        public SubjectsMediatorTests()
        {
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<SubjectMappings>());

            _serviceMock = new Mock<ISubjectsService>();
            _mapper = new Mapper(mapperConfiguration);
            _mediator = new SubjectsMediator(_serviceMock.Object, _mapper);
        }

        [Fact]
        public void CreateSubject_DependenciesCalled()
        {
            // Arrange
            var createModel = new SubjectCreateModel
            {
                Name = "Name 1"
            };

            _serviceMock.Setup(x => x.CreateSubject(It.IsAny<Subject>()))
                .Returns(new Subject { Name = createModel.Name });

            // Act
            var result = _mediator.CreateSubject(createModel);

            // Assert
            Assert.Equal(createModel.Name, result.Name);
            _serviceMock.Verify(x => x.CreateSubject(It.IsAny<Subject>()), Times.Once);
        }

        [Fact]
        public void EditSubject_DependenciesCalled()
        {
            // Arrange
            var editModel = new SubjectEditModel
            {
                Id = 1,
                Name = "Name 1"
            };

            _serviceMock.Setup(x => x.EditSubject(It.IsAny<Subject>()));

            // Act
            _mediator.EditSubject(editModel);

            // Assert
            _serviceMock.Verify(x => x.EditSubject(It.IsAny<Subject>()), Times.Once);
        }

        [Fact]
        public void ReadSubject_DependenciesCalled()
        {
            // Arrange
            const long Id = 1;
            var expectedSubject = new Subject { Id = Id, Name = "Name 1", Retired = true };
            _serviceMock.Setup(x => x.GetSubject(Id)).Returns(expectedSubject);

            // Act
            var result = _mediator.GetSubject(Id);

            // Assert
            Assert.Equal(expectedSubject.Id, result.Id);
            Assert.Equal(expectedSubject.Name, result.Name);

            _serviceMock.Verify(x => x.GetSubject(Id), Times.Once);
        }
    }
}
