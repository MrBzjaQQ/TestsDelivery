using System.Collections.Generic;

namespace TestsDelivery.BL.Services.TestPortalInstances
{
    public interface ITestPortalInstancesService
    {
        public IEnumerable<string> GetInstances();
    }
}
