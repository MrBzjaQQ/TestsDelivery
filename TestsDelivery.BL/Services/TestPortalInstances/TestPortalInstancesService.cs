using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace TestsDelivery.BL.Services.TestPortalInstances
{
    public class TestPortalInstancesService : ITestPortalInstancesService
    {
        private const string TestPortalInstancesConfigKey = "TestPortalInstances";
        private readonly IConfiguration _configuration;

        public TestPortalInstancesService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<string> GetInstances()
        {
            var section = _configuration.GetSection(TestPortalInstancesConfigKey);
            return section.GetChildren().Select(x => x.Key);
        }

        public string GetInstanceUrl(string instanceKey)
        {
            var section = _configuration.GetSection(TestPortalInstancesConfigKey);
            return section.GetChildren().Where(x => x.Key == instanceKey).Single().Value;
        }
    }
}
