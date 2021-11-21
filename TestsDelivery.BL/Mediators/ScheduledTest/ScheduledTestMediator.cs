using AutoMapper;
using TestsDelivery.BL.Models.ScheduledTest;
using TestsDelivery.BL.Services.ScheduledTest;

namespace TestsDelivery.BL.Mediators.ScheduledTest
{
    public class ScheduledTestMediator : IScheduledTestMediator
    {
        private readonly IScheduledTestService _service;
        private readonly IMapper _mapper;

        public ScheduledTestMediator(IScheduledTestService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public ScheduledTestReadModel ScheduleTest(ScheduledTestCreateModel model)
        {
            var test = _mapper.Map<Domain.ScheduledTest.ScheduledTest>(model);
            var scheduledTest = _service.ScheduleTest(test);
            // TODO: integration part - fill keycode and pin here
            return _mapper.Map<ScheduledTestReadModel>(scheduledTest);
        }

        public ScheduledTestReadModel GetTest(long id)
        {
            return _mapper.Map<ScheduledTestReadModel>(_service.GetTest(id));
        }
    }
}
