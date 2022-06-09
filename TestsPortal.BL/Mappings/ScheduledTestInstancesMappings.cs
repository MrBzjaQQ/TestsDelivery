using AutoMapper;
using TestsDelivery.UserModels.ScheduledTest;
using TestsPortal.Domain.ScheduledTests;

namespace TestsPortal.BL.Mappings
{
    public class ScheduledTestInstancesMappings: Profile
    {
        public ScheduledTestInstancesMappings()
        {
            CreateMap<Domain.ScheduledTests.ScheduledTestInstance, ScheduledTestInstanceReadModel>();
            CreateMap<DAL.Models.ScheduledTests.ScheduledTestInstance, Domain.ScheduledTests.ScheduledTestInstance>();
            CreateMap<ScheduledTestInstanceCreateModel, ScheduledTestInstanceToCreate>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.OriginalId, x => x.MapFrom(y => y.Id));
            CreateMap<ScheduledTestInstanceToCreate, DAL.Models.ScheduledTests.ScheduledTestInstance>();
        }
    }
}
