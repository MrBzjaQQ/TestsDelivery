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
                .ForMember(x => x.AdminPanelInstance, x => x.Ignore());
            CreateMap<DomainScheduledTest, DALScheduledTest>()
                .ForMember(x => x.Test, x => x.Ignore())
                .ReverseMap();
            CreateMap<DomainScheduledTest, ScheduledTestReadModel>();

            CreateMap<Domain.ScheduledTests.ScheduledTestInstance, ScheduledTestInstanceReadModel>();
            CreateMap<DAL.Models.ScheduledTests.ScheduledTestInstance, Domain.ScheduledTests.ScheduledTestInstance>();
        }
    }
}
