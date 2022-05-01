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
                .ForMember(x => x.OriginalId, x => x.MapFrom(y => y.Id));
            CreateMap<DomainScheduledTest, DALScheduledTest>()
                .ReverseMap();
            CreateMap<DomainScheduledTest, ScheduledTestReadModel>();
        }
    }
}
