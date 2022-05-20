using AutoMapper;
using System.Linq;
using TestsDelivery.Domain.ScheduledTest;
using TestsDelivery.UserModels.ScheduledTest;

namespace TestsDelivery.BL.Mappings
{
    public class ScheduledTestInstanceMappings : Profile
    {
        public ScheduledTestInstanceMappings()
        {
            CreateMap<DAL.Models.ScheduledTest.ScheduledTestInstance, Domain.ScheduledTest.ScheduledTestInstance>();
            CreateMap<Domain.ScheduledTest.ScheduledTestInstance, ScheduledTestInstanceCreateModel>();
            CreateMap<DAL.Models.ScheduledTest.ScheduledTestInstance, ScheduledTestInstanceToCreate>();
            CreateMap<ScheduledTestInstanceToCreate, ScheduledTestInstanceCreateModel>();
            CreateMap<ScheduledTestToCreate, ScheduledTestReadModel>()
                .ForMember(x => x.Candidates, x => x.MapFrom(y => y.Instances.Select(x => x.Candidate)));
        }
    }
}
