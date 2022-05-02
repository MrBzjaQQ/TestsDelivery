using AutoMapper;
using TestsDelivery.UserModels.ScheduledTest;
using TestsDelivery.BL.Shared.Providers.Communication;
using TestsDelivery.BL.Services.Communication;
using TestsDelivery.BL.Services.ScheduledTest;
using TestsDelivery.BL.Shared.Clients.Integration;
using System.Threading.Tasks;

namespace TestsDelivery.BL.Mediators.ScheduledTest
{
    public class ScheduledTestMediator : IScheduledTestMediator
    {
        private readonly ICommunicationServiceProvider _communicationServiceProvider;
        private readonly IScheduledTestService _scheduledTestService;
        private readonly IMapper _mapper;

        public ScheduledTestMediator(
            ICommunicationServiceProvider communicationServiceProvider,
            IScheduledTestService scheduledTestService,
            IMapper mapper)
        {
            _communicationServiceProvider = communicationServiceProvider;
            _scheduledTestService = scheduledTestService;
            _mapper = mapper;
        }

        public async Task<ScheduledTestReadModel> ScheduleTest(ScheduledTestCreateModel model)
        {
            var test = _mapper.Map<Domain.ScheduledTest.ScheduledTest>(model);
            var scheduledTest = _scheduledTestService.ScheduleTest(test);
            // TODO: integration part - fill keycode and pin here
            var communicationService = _communicationServiceProvider.Get<IAdminPanelCommunicationService>(model.DestinationInstance);
            await communicationService.ScheduleTest(_mapper.Map<ScheduledTestDetailedModel>(scheduledTest));
            
            return _mapper.Map<ScheduledTestReadModel>(scheduledTest);
        }

        public ScheduledTestReadModel GetTest(long id)
        {
            return _mapper.Map<ScheduledTestReadModel>(_scheduledTestService.GetTest(id));
        }
    }
}
