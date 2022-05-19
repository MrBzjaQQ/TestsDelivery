using AutoMapper;
using TestsDelivery.UserModels.AnsweredTests;
using TestsDelivery.UserModels.Questions.BaseQuestion;
using TestsDelivery.UserModels.Questions.Essay;
using TestsDelivery.UserModels.Questions.MultipleChoice;
using TestsDelivery.UserModels.Questions.SingleChoice;
using TestsDelivery.UserModels.TestProcess;
using TestsPortal.BL.Services.AdminPanelInstances;
using TestsPortal.BL.Services.Communication;
using TestsPortal.BL.Services.Questions;
using TestsPortal.BL.Services.ScheduledTests;
using TestsPortal.BL.Services.TestProcesses;
using TestsPortal.Domain.Questions;
using TestsPortal.Domain.TestProcesses;

namespace TestsPortal.BL.Mediators.TestProcesses
{
    public class TestProcessMediator : ITestProcessMediator
    {
        private readonly ITestsPortalCommunicationService _communicationService;
        private readonly ITestProcessService _testProcessService;
        private readonly IQuestionsService _questionsService;
        private readonly IAdminPanelInstancesService _instancesService;
        private readonly IScheduledTestService _scheduledTestService;
        private readonly IMapper _mapper;

        public TestProcessMediator(
            ITestProcessService testProcessService,
            IQuestionsService questionsService,
            ITestsPortalCommunicationService communicationService,
            IAdminPanelInstancesService instancesService,
            IScheduledTestService scheduledTestService,
            IMapper mapper)
        {
            _communicationService = communicationService;
            _testProcessService = testProcessService;
            _questionsService = questionsService;
            _instancesService = instancesService;
            _scheduledTestService = scheduledTestService;
            _mapper = mapper;
        }

        public async Task FinishTest(long testId)
        {
            var test = _testProcessService.FinishTest(testId);
            var instanceUrl = _instancesService.GetInstanceUrl(test.AdminPanelInstance);
            await _communicationService.FinishTest(_mapper.Map<AnsweredTestCreateModel>(test), instanceUrl);
        }

        public QuestionReadModel GetQuestionById(long questionId)
        {
            var question = _questionsService.GetById(questionId);
            return MapQuestion(question);
        }

        public IEnumerable<QuestionInListModel> GetQuestionsByTestId(long testId)
        {
            return _mapper.Map<IEnumerable<QuestionInListModel>>(_questionsService.GetByTestId(testId));
        }

        public async void StartTest(StartTestModel model)
        {
            _testProcessService.StartTest(_mapper.Map<TestCredentials>(model));
            var instanceUrl = _testProcessService.GetAdminInstanceForTest(model.TestId);
            var testId = _scheduledTestService.GetInstanceOriginalId(model.TestId);
            await _communicationService.StartTest(testId, instanceUrl);
        }

        private QuestionReadModel MapQuestion(QuestionBase question)
        {
            var type = question.GetType();
            if (type.IsAssignableFrom(typeof(SingleChoiceQuestion)))
                return _mapper.Map<ScqReadModel>(question);
            if (type.IsAssignableFrom(typeof(MultipleChoiceQuestion)))
                return _mapper.Map<McqReadModel>(question);
            if (type.IsAssignableFrom(typeof(EssayQuestion)))
                return _mapper.Map<EssayReadModel>(question);
            throw new ArgumentException("Unknown type of question", nameof(question));
        }
    }
}
