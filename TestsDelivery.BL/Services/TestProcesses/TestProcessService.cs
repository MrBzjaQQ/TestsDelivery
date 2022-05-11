using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TestsDelivery.DAL.Models.Questions;
using TestsDelivery.DAL.Repositories.Answers;
using TestsDelivery.DAL.Repositories.CandidateInScheduledTest;
using TestsDelivery.Domain.AnsweredTests;
using TestsDelivery.Domain.Questions.Answers;
using TestsDelivery.Domain.ScheduledTest;

namespace TestsDelivery.BL.Services.TestProcesses
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

        public void FinishTest(AnsweredTest test)
        {
            var instance = _instancesRepository.GetById(test.Id);
            instance.Status = (short)TestStatus.Completed;
            _instancesRepository.Update(instance);
            CreateAnswers(test, test.AnsweredQuestions);
        }

        private void CreateAnswers(AnsweredTest test, IEnumerable<Domain.Questions.Answers.AnswerBase> answers)
        {
            var choiceAnswers = new List<ChoiceAnswer>();
            var textAnswers = new List<TextAnswer>();
            var baseAnswers = new List<DAL.Models.Questions.AnswerBase>();

            foreach (var answer in answers)
            {
                var type = answer.GetType();

                if (type.IsAssignableTo(typeof(SingleChoiceAnswer)))
                    choiceAnswers.Add(MapSingleChoiceAnswer(test, answer));
                else if (type.IsAssignableTo(typeof(MultipleChoiceAnswer)))
                    choiceAnswers.AddRange(MapMultipleChoiceAnswer(test, answer));
                else if (type.IsAssignableTo(typeof(EssayAnswer)))
                    textAnswers.Add(MapEssayAnswer(test, answer));
                else
                    throw new ArgumentException("Unknown type of answer", nameof(answer));
            }

            _choiceAnswersRepository.Create(choiceAnswers);
            _textAnswersRepository.Create(textAnswers);
            baseAnswers.AddRange(choiceAnswers);
            baseAnswers.AddRange(textAnswers);
        }

        private ChoiceAnswer MapSingleChoiceAnswer(AnsweredTest test, Domain.Questions.Answers.AnswerBase answer)
        {
            var typedAnswer = answer as SingleChoiceAnswer;
            if (typedAnswer == null)
                throw new ArgumentException("Wrong type of argument", nameof(answer));

            return new ChoiceAnswer
            {
                QuestionId = typedAnswer.QuestionId,
                AnswerOptionId = typedAnswer.SelectedAnswerId,
                CandidateId = test.CandidateId,
                ScheduledTestId = test.Id
            };
        }

        private IEnumerable<ChoiceAnswer> MapMultipleChoiceAnswer(AnsweredTest test, Domain.Questions.Answers.AnswerBase answer)
        {
            var typedAnswer = answer as MultipleChoiceAnswer;
            if (typedAnswer == null)
                throw new ArgumentException("Wrong type of argument", nameof(answer));

            return typedAnswer.SelectedAnswerIds.Select(answerId => new ChoiceAnswer
            {
                QuestionId = typedAnswer.QuestionId,
                AnswerOptionId = answerId,
                CandidateId = test.CandidateId,
                ScheduledTestId = test.Id
            });
        }

        private TextAnswer MapEssayAnswer(AnsweredTest test, Domain.Questions.Answers.AnswerBase answer)
        {
            var typedAnswer = answer as EssayAnswer;
            if (typedAnswer == null)
                throw new ArgumentException("Wrong type of argument", nameof(answer));

            return new TextAnswer
            {
                QuestionId = typedAnswer.QuestionId,
                Text = typedAnswer.Text,
                CandidateId = test.CandidateId,
                ScheduledTestId = test.Id
            };
        }

        public void StartTest(long testId)
        {
            var instance = _instancesRepository.GetById(testId);
            instance.Status = (short)TestStatus.InProgress;
            _instancesRepository.Update(instance);
        }
    }
}
