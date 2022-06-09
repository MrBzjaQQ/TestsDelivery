using AutoMapper;
using TestsDelivery.UserModels.TestProcess;
using TestsPortal.Domain.TestProcesses;

namespace TestsPortal.BL.Mappings
{
    public class TestProcessMappings : Profile
    {
        public TestProcessMappings()
        {
            CreateMap<StartTestModel, TestCredentials>();
        }
    }
}
