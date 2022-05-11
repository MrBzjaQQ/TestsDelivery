using AutoMapper;
using TestsDelivery.UserModels.ScheduledTest;
using TestsDelivery.Domain.ScheduledTest;
using TestsDelivery.Domain.Test;
using System.Linq;
using TestsDelivery.Domain.Candidate;

namespace TestsDelivery.BL.Mappings
{
    public class ScheduledTestMappings : Profile
    {
        public ScheduledTestMappings()
        {
            CreateMap<ScheduledTestCreateModel, ScheduledTest>()
                .ForMember(x => x.Test, x => x.MapFrom(y => new Test { Id = y.TestId }))
                .ForMember(x => x.Candidates, y => y.MapFrom(y => y.CandidateIds.Select(x => new Candidate { Id = x })));
            CreateMap<ScheduledTest, ScheduledTestReadModel>();
            CreateMap<ScheduledTest, DAL.Models.ScheduledTest.ScheduledTest>()
                .ForMember(x => x.Test, x => x.Ignore());
            CreateMap<DAL.Models.ScheduledTest.ScheduledTest, ScheduledTest>()
                .ForMember(x => x.Test, x => x.MapFrom(y => y.Test));
            CreateMap<ScheduledTest, ScheduledTestDetailedModel>();

            CreateMap<ScheduledTestInListDto, ScheduledTestInListModel>();
            CreateMap<DAL.Models.ScheduledTest.ScheduledTestInList, ScheduledTestInListDto>();
            CreateMap<ScheduledTestsList, ScheduledTestsListModel>();
        }
    }
}
