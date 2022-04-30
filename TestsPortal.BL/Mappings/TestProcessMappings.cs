using AutoMapper;
using TestsPortal.Domain.TestProcesses;

namespace TestsPortal.BL.Mappings
{
    public class TestProcessMappings : Profile
    {
        public TestProcessMappings()
        {
            CreateMap<StartTestInfo, TestCredentials>();
        }
    }
}
