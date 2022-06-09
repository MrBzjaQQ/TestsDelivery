using AutoMapper;
using Moq;
using TestsDelivery.BL.Mappings;
using TestsDelivery.BL.Services.Candidates;
using TestsDelivery.DAL.Repositories.Candidate;
using Xunit;

namespace TestsDelivery.BL.UnitTests.Services.Candidate
{
    public class CandidateServiceTests
    {
        private readonly Mapper _mapper;
        private readonly CandidateService _candidateService;
        private readonly Mock<ICandidateRepository> _candidateRepositoryMock;

        public CandidateServiceTests()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<CandidateMappings>());
            _mapper = new Mapper(mapperConfig);
            _candidateRepositoryMock = new Mock<ICandidateRepository>();
            _candidateService = new CandidateService(_candidateRepositoryMock.Object, _mapper);
        }

        [Fact]
        public void CreateCandidate_DependenciesCalled()
        {
            // Arrange
            var createModel = new Domain.Candidate.Candidate
            {
                Email = "abc@example.com",
                FirstName = "First Name",
                LastName = "Last Name"
            };

            _candidateRepositoryMock.Setup(x => x.Create(It.IsAny<DAL.Models.Candidate.Candidate>()));

            // Act
            var readModel = _candidateService.CreateCandidate(createModel);

            // Assert
            Assert.Equal(createModel.FirstName, readModel.FirstName);
            Assert.Equal(createModel.LastName, readModel.LastName);
            Assert.Equal(createModel.Email, readModel.Email);

            _candidateRepositoryMock.Verify(x => x.Create(It.IsAny<DAL.Models.Candidate.Candidate>()), Times.Once);
        }

        [Fact]
        public void EditCandidate_DependenciesCalled()
        {
            // Arrange
            var editModel = new Domain.Candidate.Candidate
            {
                Email = "abc@example.com",
                FirstName = "First Name",
                LastName = "Last Name"
            };

            _candidateRepositoryMock.Setup(x => x.Update(It.IsAny<DAL.Models.Candidate.Candidate>()));

            // Act
            _candidateService.EditCandidate(editModel);

            // Assert
            _candidateRepositoryMock.Verify(x => x.Update(It.IsAny<DAL.Models.Candidate.Candidate>()), Times.Once);
        }

        [Fact]
        public void GetCandidate_DependenciesCalled()
        {
            // Arrange
            const long candidateId = 1;
            _candidateRepositoryMock.Setup(x => x.GetById(candidateId));

            // Act
            _candidateService.GetCandidate(candidateId);

            // Assert
            _candidateRepositoryMock.Verify(x => x.GetById(candidateId), Times.Once);
        }
    }
}
