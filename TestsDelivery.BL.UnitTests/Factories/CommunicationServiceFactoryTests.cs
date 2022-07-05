using AutoMapper;
using Moq;
using TestsDelivery.BL.Factories;
using TestsDelivery.BL.Services.Communication;
using TestsDelivery.BL.Shared.Clients.Integration;
using Xunit;

namespace TestsDelivery.BL.UnitTests.Factories
{
    public class CommunicationServiceFactoryTests
    {
        private IMapper _mapper;
        private Mock<IIntegrationApiClient> _apiClientMock;
        private CommunicationServiceFactory _factory;

        public CommunicationServiceFactoryTests()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg => { }));
            _apiClientMock = new Mock<IIntegrationApiClient>();
            _factory = new CommunicationServiceFactory(_mapper);
        }

        [Fact]
        public void Create_TypeProvided_InstanceCreated()
        {
            // Arrange
            const string instanceUrl = "https://www.example.com";

            // Act
            var createdService = _factory.Create<IAdminPanelCommunicationService>(_apiClientMock.Object, instanceUrl);

            // Assert
            Assert.IsType<AdminPanelCommunicationService>(createdService);
        }
    }
}
