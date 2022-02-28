using AutoMapper;
using TestsDelivery.BL.Models.ScheduledTest;
using TestsDelivery.BL.Services.Communication;
using TestsDelivery.BL.Services.ScheduledTest;

namespace TestsDelivery.BL.Mediators.ScheduledTest
{
    public class ScheduledTestMediator : IScheduledTestMediator
    {
        private readonly IAdminPanelCommunicationService _communicationService;
        private readonly IScheduledTestService _scheduledTestService;
        private readonly IMapper _mapper;

        public ScheduledTestMediator(
            IAdminPanelCommunicationService communicationService,
            IScheduledTestService scheduledTestService,
            IMapper mapper)
        {
            _communicationService = communicationService;
            _scheduledTestService = scheduledTestService;
            _mapper = mapper;
        }

        public ScheduledTestReadModel ScheduleTest(ScheduledTestCreateModel model)
        {
            var test = _mapper.Map<Domain.ScheduledTest.ScheduledTest>(model);
            var scheduledTest = _scheduledTestService.ScheduleTest(test);
            _communicationService.ScheduleTest(scheduledTest);
            // TODO: integration part - fill keycode and pin here
            return _mapper.Map<ScheduledTestReadModel>(scheduledTest);
        }

        public ScheduledTestReadModel GetTest(long id)
        {
            return _mapper.Map<ScheduledTestReadModel>(_scheduledTestService.GetTest(id));
        }
    }
}
