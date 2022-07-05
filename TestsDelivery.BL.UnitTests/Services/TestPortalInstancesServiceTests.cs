using Microsoft.Extensions.Configuration;
using Moq;
using System.Linq;
using TestsDelivery.BL.Services.TestPortalInstances;
using Xunit;

namespace TestsDelivery.BL.UnitTests.Services
{
    public class TestPortalInstancesServiceTests
    {
        private const string TestPortalInstancesConfigKey = "TestPortalInstances";

        private Mock<IConfiguration> _configMock;
        private Mock<IConfigurationSection> _configSectionMock;
        private TestPortalInstancesService _service;

        public TestPortalInstancesServiceTests()
        {
            _configMock = new Mock<IConfiguration>();
            _configSectionMock = new Mock<IConfigurationSection>();
            _service = new TestPortalInstancesService(_configMock.Object);
        }

        [Fact]
        public void GetInstances_Success()
        {
            // Arrange
            var keys = new string[] { "key1", "key2" };
            var section1 = new Mock<IConfigurationSection>();
            var section2 = new Mock<IConfigurationSection>();

            section1.Setup(x => x.Key).Returns(keys[0]);
            section2.Setup(x => x.Key).Returns(keys[1]);

            var instances = new [] { section1.Object, section2.Object };

            _configSectionMock.Setup(x => x.GetChildren()).Returns(instances);

            _configMock.Setup(x => x.GetSection(TestPortalInstancesConfigKey))
                .Returns(_configSectionMock.Object);

            // Act
            var resultKeys = _service.GetInstances().ToArray();

            // Assert
            Assert.Equal(keys[0], resultKeys[0]);
            Assert.Equal(keys[1], resultKeys[1]);

            _configMock.Verify(x => x.GetSection(TestPortalInstancesConfigKey), Times.Once);
            _configSectionMock.Verify(x => x.GetChildren(), Times.Once);
        }

        [Fact]
        public void GetInstanceUrl_Success()
        {
            // Arrange
            const string instanceKey = "key1";
            const string instanceUrl = "https://www.example.com";
            var keys = new string[] { instanceKey, "key2" };
            var section1 = new Mock<IConfigurationSection>();
            var section2 = new Mock<IConfigurationSection>();

            section1.Setup(x => x.Key).Returns(keys[0]);
            section1.Setup(x => x.Value).Returns(instanceUrl);
            section2.Setup(x => x.Key).Returns(keys[1]);
            section2.Setup(x => x.Value).Returns("");

            var instances = new[] { section1.Object, section2.Object };

            _configSectionMock.Setup(x => x.GetChildren()).Returns(instances);

            _configMock.Setup(x => x.GetSection(TestPortalInstancesConfigKey))
                .Returns(_configSectionMock.Object);

            // Act
            var url = _service.GetInstanceUrl(instanceKey).ToArray();

            // Assert
            Assert.Equal(instanceUrl, url);

            _configMock.Verify(x => x.GetSection(TestPortalInstancesConfigKey), Times.Once);
            _configSectionMock.Verify(x => x.GetChildren(), Times.Once);
        }
    }
}
