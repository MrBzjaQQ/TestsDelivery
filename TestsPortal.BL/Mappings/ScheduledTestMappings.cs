using AutoMapper;
using DomainScheduledTest = TestsPortal.Domain.ScheduledTests.ScheduledTest;
using DALScheduledTest = TestsPortal.DAL.Models.ScheduledTests.ScheduledTest;
using TestsDelivery.UserModels.ScheduledTest;

namespace TestsPortal.BL.Mappings
{
    public class ScheduledTestMappings : Profile
    {
        public ScheduledTestMappings()
        {
            CreateMap<ScheduledTestDetailedModel, DomainScheduledTest>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.OriginalId, x => x.MapFrom(y => y.Id))
                // TODO: Fill keycode and password
                .ForMember(x => x.Pin, x => x.MapFrom(y => "ABCDEF"))
                .ForMember(x => x.Keycode, x => x.MapFrom(y => "ABCDEF"));
            CreateMap<DomainScheduledTest, DALScheduledTest>()
                .ReverseMap();
            CreateMap<DomainScheduledTest, ScheduledTestReadModel>();
        }
    }
}
