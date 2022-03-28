using AutoMapper;
using TestsDelivery.UserModels.Test;
using TestsPortal.Domain.Tests;

namespace TestsPortal.BL.Mappings
{
    public class TestMappings : Profile
    {
        public TestMappings()
        {
            CreateMap<TestDetailedModel, Test>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.OriginalId, x => x.MapFrom(y => y.Id));
        }
    }
}
