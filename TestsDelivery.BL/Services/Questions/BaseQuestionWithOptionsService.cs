using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using TestsDelivery.DAL.Repositories.AnswerOptions;
using TestsDelivery.DAL.Repositories.Questions;
using TestsDelivery.Domain.Questions;
using TestsDelivery.Domain.Subject;
using AnswerOptionDALModel = TestsDelivery.DAL.Models.Questions.AnswerOption;
using AnswerOptionDomainModel = TestsDelivery.Domain.Questions.AnswerOption;

namespace TestsDelivery.BL.Services.Questions
{
    public abstract class BaseQuestionWithOptionsService<TDomainQuestion> : BaseQuestionService<TDomainQuestion>, IBaseQuestionService<TDomainQuestion>
        where TDomainQuestion: QuestionWithOptionsBase, new()
    {
        private readonly IAnswerOptionsRepository _answerOptionsRepository;

        protected BaseQuestionWithOptionsService(
            IQuestionsRepository questionsRepository,
            IAnswerOptionsRepository answerOptionsRepository,
            IMapper mapper,
            QuestionType questionType)
            : base(questionsRepository, mapper, questionType)
        {
            _answerOptionsRepository = answerOptionsRepository;
        }

        public override TDomainQuestion CreateQuestion(TDomainQuestion question)
        {
            var questionDataModel = base.CreateQuestion(question);

            var answerOptions = MapAnswerOptionsToDataModels(question.AnswerOptions, questionDataModel.Id).ToList();
            _answerOptionsRepository.CreateAnswerOptions(answerOptions);

            return MapQuestionToDomain(questionDataModel, answerOptions);
        }

        public override void DeleteQuestion(long id)
        {
            _answerOptionsRepository.DeleteAnswerOptionsByQuestionId(id);
            base.DeleteQuestion(id);
        }

        public override void EditQuestion(TDomainQuestion question)
        {
            var answerOptionsToEdit = MapAnswerOptionsToDataModels(
                question.AnswerOptions.Where(x => x.Id != 0),
                question.Id).ToArray();
            var answerOptionsToCreate = MapAnswerOptionsToDataModels(
                question.AnswerOptions.Where(x => x.Id == 0),
                question.Id).ToArray();

            _answerOptionsRepository.EditAnswerOptions(answerOptionsToEdit);
            _answerOptionsRepository.CreateAnswerOptions(answerOptionsToCreate);

            var answerOptions = _answerOptionsRepository.GetAnswerOptionsForQuestion(question.Id);
            var currentAnswerOptionsIds = answerOptionsToCreate.Concat(answerOptionsToEdit).Select(x => x.Id).ToHashSet();
            var answerOptionsToDelete = answerOptions
                .Select(x => x.Id)
                .Where(id => !currentAnswerOptionsIds.Contains(id));

            _answerOptionsRepository.DeleteAnswerOptions(answerOptionsToDelete);

            base.EditQuestion(question);
        }

        public override TDomainQuestion GetQuestion(long id)
        {
            var question = base.GetQuestion(id);
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

        private TDomainQuestion MapQuestionToDomain(TDomainQuestion question, IEnumerable<AnswerOptionDALModel> options)
        {
            return new TDomainQuestion
            {
                AnswerOptions = Mapper.Map<IEnumerable<AnswerOptionDomainModel>>(options),
                Id = question.Id,
                Name = question.Name,
                Text = question.Text,
                Subject = Mapper.Map<Subject>(question.Subject),
                Type = QuestionType
            };
        }
    }
}
