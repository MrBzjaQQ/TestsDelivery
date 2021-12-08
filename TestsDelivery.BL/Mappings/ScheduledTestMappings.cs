﻿using AutoMapper;
using TestsDelivery.BL.Models.ScheduledTest;
using TestsDelivery.Domain.Candidate;
using TestsDelivery.Domain.ScheduledTest;
using TestsDelivery.Domain.Test;

namespace TestsDelivery.BL.Mappings
{
    public class ScheduledTestMappings : Profile
    {
        public ScheduledTestMappings()
        {
            CreateMap<ScheduledTestCreateModel, ScheduledTest>()
                .ForMember(x => x.Status, x => x.MapFrom(y => TestStatus.NotStarted))
                .ForMember(x => x.Candidate, x => x.MapFrom(y => new Candidate { Id = y.CandidateId }))
                .ForMember(x => x.Test, x => x.MapFrom(y => new Test { Id = y.TestId }));
            CreateMap<ScheduledTest, ScheduledTestReadModel>();
            CreateMap<ScheduledTest, DAL.Models.ScheduledTest.ScheduledTest>()
                .ForMember(x => x.Test, x => x.Ignore())
                .ForMember(x => x.Candidate, x => x.Ignore());
            CreateMap<DAL.Models.ScheduledTest.ScheduledTest, ScheduledTest>();
        }
    }
}