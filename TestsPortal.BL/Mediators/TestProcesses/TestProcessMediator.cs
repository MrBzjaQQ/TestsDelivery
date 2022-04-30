using AutoMapper;
using TestsDelivery.UserModels.Questions.BaseQuestion;
using TestsDelivery.UserModels.TestProcess;
using TestsPortal.BL.Services.Questions;
using TestsPortal.BL.Services.TestProcesses;
using TestsPortal.Domain.TestProcesses;

namespace TestsPortal.BL.Mediators.TestProcesses
{
    public class TestProcessMediator : ITestProcessMediator
    {
        private readonly ITestProcessService _testProcessService;
        private readonly IQuestionsService _questionsService;
        private readonly IMapper _mapper;

        public TestProcessMediator(
            ITestProcessService testProcessService,
            IQuestionsService questionsService,
            IMapper mapper)
        {
            _testProcessService = testProcessService;
            _questionsService = questionsService;
            _mapper = mapper;
        }

        public void FinishTest(long testId)
        {
            _testProcessService.FinishTest(testId);
        }

        public QuestionReadModel GetQuestionById(long questionId)
        {
            return _mapper.Map<QuestionReadModel>(_questionsService.GetById(questionId));
        }

        public IEnumerable<QuestionInListModel> GetQuestionsByTestId(long testId)
        {
            return _mapper.Map<IEnumerable<QuestionInListModel>>(_questionsService.GetByTestId(testId));
        }

        public void StartTest(StartTestModel model)
        {
            _testProcessService.StartTest(_mapper.Map<TestCredentials>(model));
        }
    }
}
