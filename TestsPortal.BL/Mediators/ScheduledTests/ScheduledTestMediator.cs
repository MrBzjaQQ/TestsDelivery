using AutoMapper;
using TestsDelivery.UserModels.ScheduledTest;
using TestsPortal.BL.Services.EmailServices;
using TestsPortal.BL.Services.ScheduledTests;
using TestsPortal.Domain.ScheduledTests;

namespace TestsPortal.BL.Mediators.ScheduledTests
{
    public class ScheduledTestMediator : IScheduledTestMediator
    {
        private readonly IScheduledTestService _scheduledTestService;
        private readonly ICandidateNotificationService _notificationService;
        private readonly IMapper _mapper;

        public ScheduledTestMediator(
            IScheduledTestService scheduledTestService,
            ICandidateNotificationService notificationService,
            IMapper mapper)
        {
            _scheduledTestService = scheduledTestService;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        public IEnumerable<ScheduledTestInstanceReadModel> ScheduleTest(ScheduledTestDetailedModel model, string host)
        {
            var inputModel = _mapper.Map<ScheduledTest>(model);
            inputModel.AdminPanelInstance = host;
            var outputModels = _scheduledTestService.ScheduleTest(inputModel);
            _notificationService.NotifyCandidates(outputModels.Select(x => x.Candidate), "TODO: think about building a message");
            return _mapper.Map<IEnumerable<ScheduledTestInstanceReadModel>>(outputModels);
        }
    }
}
