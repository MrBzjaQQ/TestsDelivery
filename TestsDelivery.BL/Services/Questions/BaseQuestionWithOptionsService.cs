using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TestsDelivery.DAL.Models.Questions;
using TestsDelivery.DAL.Repositories.AnswerOptions;
using TestsDelivery.DAL.Repositories.Questions;
using TestsDelivery.Domain.Questions;
using TestsDelivery.Domain.Subject;
using AnswerOptionDALModel = TestsDelivery.DAL.Models.Questions.AnswerOption;
using AnswerOptionDomainModel = TestsDelivery.Domain.Questions.AnswerOption;

namespace TestsDelivery.BL.Services.Questions
{
    public abstract class BaseQuestionWithOptionsService<TDomainQuestion> : IBaseQuestionService<TDomainQuestion>
        where TDomainQuestion: QuestionWithOptionsBase, new()
    {
        private readonly IQuestionsRepository _questionsRepository;
        private readonly IAnswerOptionsRepository _answerOptionsRepository;
        private readonly IMapper _mapper;
        private readonly QuestionType _questionType;

        protected BaseQuestionWithOptionsService(
            IQuestionsRepository questionsRepository,
            IAnswerOptionsRepository answerOptionsRepository,
            IMapper mapper,
            QuestionType questionType)
        {
            _questionsRepository = questionsRepository;
            _answerOptionsRepository = answerOptionsRepository;
            _mapper = mapper;
            _questionType = questionType;
        }

        public TDomainQuestion CreateQuestion(TDomainQuestion question)
        {
            var questionDataModel = _mapper.Map<Question>(question);
            _questionsRepository.CreateQuestion(questionDataModel);

            var answerOptions = MapAnswerOptionsToDataModels(question.AnswerOptions, questionDataModel.Id).ToList();
            _answerOptionsRepository.CreateAnswerOptions(answerOptions);

            return MapQuestionToDomain(_questionsRepository.GetQuestion(questionDataModel.Id), answerOptions);
        }

        public void EditQuestion(TDomainQuestion question)
        {
            var questionDataModel = _mapper.Map<Question>(question);
            _questionsRepository.EditQuestion(questionDataModel);

            var answerOptionsToEdit = MapAnswerOptionsToDataModels(
                question.AnswerOptions.Where(x => x.Id != 0),
                questionDataModel.Id).ToArray();
            var answerOptionsToCreate = MapAnswerOptionsToDataModels(
                question.AnswerOptions.Where(x => x.Id == 0),
                questionDataModel.Id).ToArray();

            _answerOptionsRepository.EditAnswerOptions(answerOptionsToEdit);
            _answerOptionsRepository.CreateAnswerOptions(answerOptionsToCreate);

            var answerOptions = _answerOptionsRepository.GetAnswerOptionsForQuestion(questionDataModel.Id);
            var currentAnswerOptionsIds = answerOptionsToCreate.Concat(answerOptionsToEdit).Select(x => x.Id).ToHashSet();
            var answerOptionsToDelete = answerOptions
                .Select(x => x.Id)
                .Where(id => !currentAnswerOptionsIds.Contains(id));

            _answerOptionsRepository.DeleteAnswerOptions(answerOptionsToDelete);
        }

        public TDomainQuestion GetQuestion(long id)
        {
            var question = _questionsRepository.GetQuestion(id, (short)_questionType);
            var answerOptions = _answerOptionsRepository.GetAnswerOptionsForQuestion(id);
            return MapQuestionToDomain(question, answerOptions);
        }

        private IEnumerable<AnswerOptionDALModel> MapAnswerOptionsToDataModels(
            IEnumerable<AnswerOptionDomainModel> options,
            long questionId)
        {
            foreach (var opt in options)
            {
                yield return new AnswerOptionDALModel
                {
                    Id = opt.Id,
                    IsCorrect = opt.IsCorrect,
                    QuestionId = questionId,
                    Text = opt.Text
                };
            }
        }

        private TDomainQuestion MapQuestionToDomain(Question question, IEnumerable<AnswerOptionDALModel> options)
        {
            return new TDomainQuestion
            {
                AnswerOptions = _mapper.Map<IEnumerable<AnswerOptionDomainModel>>(options),
                Id = question.Id,
                Name = question.Name,
                Text = question.Text,
                Subject = _mapper.Map<Subject>(question.Subject),
                Type = QuestionType.MultipleChoice
            };
        }
    }
}
