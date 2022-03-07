using AutoMapper;
using TestsDelivery.UserModels.ScheduledTest;
using TestsPortal.BL.Services.ScheduledTest;

namespace TestsPortal.BL.Mediators.ScheduledTest
{
    public class ScheduledTestMediator : IScheduledTestMediator
    {
        private readonly IScheduledTestService _scheduledTestService;
        private readonly IMapper _mapper;

        public ScheduledTestMediator(IScheduledTestService scheduledTestService, IMapper mapper)
        {
            _scheduledTestService = scheduledTestService;
            _mapper = mapper;
        }

        public ScheduledTestReadModel ScheduleTest(ScheduledTestDetailedModel model)
        {
            throw new NotImplementedException();
        }
    }
}
