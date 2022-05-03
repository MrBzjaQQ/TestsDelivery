using AutoMapper;
using TestsDelivery.BL.Services.TestProcesses;
using TestsDelivery.Domain.AnsweredTests;
using TestsDelivery.UserModels.AnsweredTests;

namespace TestsDelivery.BL.Mediators.TestProcesses
{
    public class TestProcessMediator : ITestProcessMediator
    {
        private readonly ITestProcessService _testProcessService;
        private readonly IMapper _mapper;

        public TestProcessMediator(ITestProcessService testProcessService, IMapper mapper)
        {
            _testProcessService = testProcessService;
            _mapper = mapper;
        }

        public void FinishTest(AnsweredTestCreateModel answeredTest)
        {
            var answerDomain = _mapper.Map<AnsweredTest>(answeredTest);
            _testProcessService.FinishTest(answerDomain);
        }
    }
}
