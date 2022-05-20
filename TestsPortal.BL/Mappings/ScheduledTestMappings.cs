using AutoMapper;
using DomainScheduledTest = TestsPortal.Domain.ScheduledTests.ScheduledTest;
using DALScheduledTest = TestsPortal.DAL.Models.ScheduledTests.ScheduledTest;
using TestsDelivery.UserModels.ScheduledTest;
using TestsPortal.Domain.ScheduledTests;

namespace TestsPortal.BL.Mappings
{
    public class ScheduledTestMappings : Profile
    {
        public ScheduledTestMappings()
        {
            CreateMap<ScheduledTestDetailedModel, DomainScheduledTest>()
                .ForMember(x => x.Id, y => y.Ignore())
                .ForMember(x => x.OriginalId, x => x.MapFrom(y => y.Id))
                .ForMember(x => x.Candidates, x => x.MapFrom(y => y.Instances.Select(x => x.Candidate)))
                .ForMember(x => x.AdminPanelInstance, x => x.Ignore());
            CreateMap<DomainScheduledTest, DALScheduledTest>()
                .ForMember(x => x.Test, x => x.Ignore())
                .ReverseMap();
            CreateMap<DomainScheduledTest, ScheduledTestReadModel>();

            CreateMap<ScheduledTestDetailedModel, ScheduledTestToCreate>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.OriginalId, x => x.MapFrom(y => y.Id));
            CreateMap<ScheduledTestToCreate, DALScheduledTest>()
                .ForMember(x => x.TestId, x => x.MapFrom(y => y.Test.Id))
                .ForMember(x => x.Test, x => x.Ignore());

        }
    }
}
