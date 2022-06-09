using AutoMapper;
using Moq;
using TestsDelivery.BL.Mappings;
using TestsDelivery.BL.Services.Subjects;
using TestsDelivery.DAL.Repositories.Subjects;
using TestsDelivery.DAL.Models.Subject;
using Xunit;

namespace TestsDelivery.BL.UnitTests.Services.Subjects
{
    public class SubjectsServiceTests
    {
        private readonly Mock<ISubjectsRepository> _repositoryMock;

        private readonly SubjectsService _service;

        public SubjectsServiceTests()
        {
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<SubjectMappings>());

            _repositoryMock = new Mock<ISubjectsRepository>();
            _service = new SubjectsService(_repositoryMock.Object, new Mapper(mapperConfiguration));
        }

        [Fact]
        public void CreateSubject_DataIsProvided_DependenciesCalled()
        {
            // Act
            _service.CreateSubject(new Domain.Subject.Subject());

            // Assert
            _repositoryMock.Verify(x => x.Create(It.IsAny<Subject>()), Times.Once);
        }

        [Fact]
        public void EditSubject_DataIsProvided_DependenciesCalled()
        {
            // Act
            _service.EditSubject(new Domain.Subject.Subject());

            // Assert
            _repositoryMock.Verify(x => x.Update(It.IsAny<Subject>()), Times.Once);
        }

        [Fact]
        public void GetSubject_DataIsProvided_DependenciesCalled()
        {
            // Arrange
            const int providedId = 12;

            var expectedResult = new Subject
            {
                Id = providedId,
                Name = "Some name",
                Retired = true
            };

            _repositoryMock.Setup(x => x.GetById(providedId))
                .Returns(expectedResult);

            // Act
            var subject = _service.GetSubject(providedId);

            // Assert
            Assert.Equal(subject.Id, expectedResult.Id);
            Assert.Equal(subject.Name, expectedResult.Name);
            Assert.Equal(subject.Retired, expectedResult.Retired);

            _repositoryMock.Verify(x => x.GetById(providedId), Times.Once);
        }
    }
}
