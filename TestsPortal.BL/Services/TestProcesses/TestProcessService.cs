using AutoMapper;
using TestsPortal.BL.Exceptions;
using TestsPortal.BL.FilterBuilders;
using TestsPortal.DAL.Repositories.Answers;
using TestsPortal.DAL.Repositories.ScheduledTestInstances;
using TestsPortal.Domain.AnsweredTests;
using TestsPortal.Domain.Questions.AnsweredQuestions;
using TestsPortal.Domain.ScheduledTests;
using TestsPortal.Domain.TestProcesses;

namespace TestsPortal.BL.Services.TestProcesses
{
    public class TestProcessService : ITestProcessService
    {
        private readonly IScheduledTestInstancesRepository _instancesRepository;
        private readonly IChoiceAnswersRepository _choiceAnswersRepository;
        private readonly ITextAnswersRepository _textAnswersRepository;
        private readonly IMapper _mapper;

        public TestProcessService(
            IScheduledTestInstancesRepository instancesRepository,
            IChoiceAnswersRepository choiceAnswersRepository,
            ITextAnswersRepository textAnswersRepository,
            IMapper mapper)
        {
            _instancesRepository = instancesRepository;
            _choiceAnswersRepository = choiceAnswersRepository;
            _textAnswersRepository = textAnswersRepository;
            _mapper = mapper;
        }

        public AnsweredTest FinishTest(long testId)
        {
            var test = _instancesRepository.GetById(testId);
            test.Status = (short)TestStatus.Completed;
            _instancesRepository.Update(test);
            var answeredTest = _mapper.Map<AnsweredTest>(test);
            answeredTest.AnsweredQuestions = GetAnsweredQuestions(testId);
            return answeredTest;
        }

        public string GetAdminInstanceForTest(long testId)
        {
            return _instancesRepository.GetAdminInstanceForTest(testId);
        }

        public void StartTest(TestCredentials credentials)
        {
            var filter = new ScheduledTestsInstancesFilterBuilder()
                .ByTestId(credentials.TestId)
                .ByCandidateId(credentials.CandidateId)
                .ByKeycode(credentials.Keycode)
                .ByPin(credentials.Pin)
                .Build();

            var tests = _instancesRepository.GetByFilter(filter);

            if (tests.Count() == 0)
                throw new CandidateAuthenticationException();

            var test = tests.Single();
            test.Status = (short)TestStatus.InProgress;
            _instancesRepository.Update(test);
        }

        private IEnumerable<AnsweredQuestionBase> GetAnsweredQuestions(long testId)
        {
            var targetCollection = new List<AnsweredQuestionBase>();
            targetCollection.AddRange(GetProcessedChoiceAnswers(testId));
            targetCollection.AddRange(GetProcessedTextAnswers(testId));
            return targetCollection.OrderBy(x => x.QuestionId);
        }

        private IEnumerable<AnsweredQuestionBase> GetProcessedChoiceAnswers(long testId)
        {
            var choiceAnswers = _choiceAnswersRepository.GetByTestId(testId);
            var targetCollection = new List<AnsweredQuestionBase>();
            var groupedChoiceAnswers = choiceAnswers.GroupBy(x => x.QuestionId);
            foreach (var choices in groupedChoiceAnswers)
            {
                if (choices.Count() == 1)
                {
                    var answer = choices.First();
                    targetCollection.Add(new AnsweredSingleChoice
                    {
                        CandidateId = answer.CandidateId,
                        QuestionId = answer.QuestionId,
                        ScheduledTestId = answer.ScheduledTestId,
                        SelectedAnswerId = answer.ScheduledTestId
                    });
                }
                else
                {
                    var answer = choices.First();
                    targetCollection.Add(new AnsweredMultipleChoice
                    {
                        CandidateId = answer.CandidateId,
                        QuestionId = answer.QuestionId,
                        ScheduledTestId = answer.ScheduledTestId,
                        SelectedAnswerIds = choices.Select(x => x.AnswerId)
                    });
                }
            }

            return targetCollection;
        }

        private IEnumerable<AnsweredQuestionBase> GetProcessedTextAnswers(long testId)
        {
            var textAnswers = _textAnswersRepository.GetByTestId(testId);
            return textAnswers.Select(x => new AnsweredEssay
            {
                CandidateId = x.CandidateId,
                QuestionId = x.QuestionId,
                ScheduledTestId = x.ScheduledTestId,
                Text = x.Text
            });
        }
    }
}
