using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TestsDelivery.DAL.Models.Questions;
using TestsDelivery.DAL.Repositories.AnswerOptions;
using TestsDelivery.DAL.Repositories.Questions;
using TestsDelivery.Domain.Questions;
using TestsDelivery.Domain.Subject;
using AnswerOption = TestsDelivery.DAL.Models.Questions.AnswerOption;

namespace TestsDelivery.BL.Services.Questions.SingleChoice
{
    public class ScqService : IScqService
    {
        private readonly IQuestionsRepository _questionsRepository;
        private readonly IAnswerOptionsRepository _answerOptionsRepository;
        private readonly IMapper _mapper;

        public ScqService(
            IQuestionsRepository questionsRepository,
            IAnswerOptionsRepository answerOptionsRepository,
            IMapper mapper)
        {
            _questionsRepository = questionsRepository;
            _answerOptionsRepository = answerOptionsRepository;
            _mapper = mapper;
        }

        public SingleChoiceQuestion CreateQuestion(SingleChoiceQuestion question)
        {
            var questionDataModel = _mapper.Map<Question>(question);
            _questionsRepository.CreateQuestion(questionDataModel);

            var answerOptions = MapAnswerOptionsToDataModels(question.AnswerOptions, questionDataModel.Id);
            _answerOptionsRepository.CreateAnswerOptions(answerOptions);

            return MapQuestionToScq(questionDataModel, answerOptions);
        }

        public void EditQuestion(SingleChoiceQuestion question)
        {
            var questionDataModel = _mapper.Map<Question>(question);
            _questionsRepository.EditQuestion(questionDataModel);

            var answerOptionsToEdit = MapAnswerOptionsToDataModels(
                question.AnswerOptions.Where(x => x.Id != 0),
                questionDataModel.Id);
            var answerOptionsToCreate = MapAnswerOptionsToDataModels(
                question.AnswerOptions.Where(x => x.Id == 0),
                questionDataModel.Id);

            _answerOptionsRepository.EditAnswerOptions(answerOptionsToEdit);
            _answerOptionsRepository.CreateAnswerOptions(answerOptionsToCreate);
        }

        public SingleChoiceQuestion GetQuestion(long id)
        {
            return _mapper.Map<SingleChoiceQuestion>(_questionsRepository.GetQuestion(id));
        }

        private IEnumerable<AnswerOption> MapAnswerOptionsToDataModels(
            IEnumerable<Domain.Questions.AnswerOption> options,
            long questionId)
        {
            foreach (var opt in options)
            {
                yield return new AnswerOption
                {
                    Id = opt.Id,
                    IsCorrect = opt.IsCorrect,
                    QuestionId = questionId,
                    Text = opt.Text
                };
            }
        }

        private SingleChoiceQuestion MapQuestionToScq(Question question, IEnumerable<AnswerOption> options)
        {
            return new SingleChoiceQuestion
            {
                AnswerOptions = _mapper.Map<IEnumerable<Domain.Questions.AnswerOption>>(options),
                Id = question.Id,
                Name = question.Name,
                Question = question.Text,
                Subject = _mapper.Map<Subject>(question.Subject)
            };
        }
    }
}
