using AutoMapper;
using Moq;
using TestsDelivery.BL.Mappings;
using TestsDelivery.BL.Mediators.Candidate;
using TestsDelivery.UserModels.Candidate;
using TestsDelivery.BL.Services.Candidates;
using Xunit;

namespace TestsDelivery.BL.UnitTests.Mediators
{
    public class CandidateTests
    {
        private readonly Mock<ICandidateService> _serviceMock;
        private readonly IMapper _mapper;
        private readonly CandidateMediator _mediator;
        public CandidateTests()
        {
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<CandidateMappings>());
            _serviceMock = new Mock<ICandidateService>();
            _mapper = new Mapper(mapperConfiguration);
            _mediator = new CandidateMediator(_serviceMock.Object, _mapper);
        }

        [Fact]
        public void CreateCandidate_DependenciesCalled()
        {
            // Arrange
            var createModel = new CandidateCreateModel 
            { 
                Email = "abc@example.com",
                FirstName = "FirstName",
                LastName = "LastName"
            };

            var createdByService = new Domain.Candidate.Candidate
            {
                Email = createModel.Email,
                FirstName = createModel.FirstName,
                LastName = createModel.LastName
            };

            _serviceMock.Setup(x => x.CreateCandidate(It.IsAny<Domain.Candidate.Candidate>()))
                .Returns(createdByService);

            // Act
            var result = _mediator.CreateCandidate(createModel);

            // Assert
            Assert.Equal(createModel.Email, result.Email);
            Assert.Equal(createModel.FirstName, result.FirstName);
            Assert.Equal(createModel.LastName, result.LastName);

            _serviceMock.Verify(x => x.CreateCandidate(It.IsAny<Domain.Candidate.Candidate>()), Times.Once);
        }

        [Fact]
        public void EditCandidate_DependenciesCalled()
        {
            // Arrange
            var editModel = new CandidateEditModel
            {
                Email = "abc@example.com",
                FirstName = "FirstName",
                LastName = "LastName",
                Id = 1
            };

            _serviceMock.Setup(x => x.EditCandidate(It.IsAny<Domain.Candidate.Candidate>()));

            // Act
            _mediator.EditCandidate(editModel);

            // Assert
            _serviceMock.Verify(x => x.EditCandidate(It.IsAny<Domain.Candidate.Candidate>()), Times.Once);
        }

        [Fact]
        public void GetCandidate_DependenciesCalled()
        {
            // Arrange
            const long Id = 1;
            var modelFromService = new Domain.Candidate.Candidate
            {
                Id = Id,
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "abc@example.com"
            };

            _serviceMock.Setup(x => x.GetCandidate(Id))
                .Returns(modelFromService);

            // Act
            var result = _mediator.GetCandidate(Id);

            // Assert
            Assert.Equal(modelFromService.FirstName, result.FirstName);
            Assert.Equal(modelFromService.LastName, result.LastName);
            Assert.Equal(modelFromService.Email, result.Email);

            _serviceMock.Verify(x => x.GetCandidate(Id), Times.Once);
        }
    }
}
