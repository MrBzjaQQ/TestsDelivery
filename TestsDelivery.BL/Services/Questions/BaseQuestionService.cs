using AutoMapper;
using TestsDelivery.DAL.Models.Questions;
using TestsDelivery.DAL.Repositories.Questions;
using TestsDelivery.Domain.Questions;

namespace TestsDelivery.BL.Services.Questions
{
    public abstract class BaseQuestionService<TDomainQuestion, TDataQuestion> : IBaseQuestionService<TDomainQuestion>
        where TDomainQuestion : QuestionBase
        where TDataQuestion: Question
    {
        private readonly IQuestionsRepository _repository;
        private readonly IMapper _mapper;
        private readonly QuestionType _questionType;

        protected BaseQuestionService(IQuestionsRepository repository, IMapper mapper, QuestionType questionType)
        {
            _repository = repository;
            _mapper = mapper;
            _questionType = questionType;
        }

        public TDomainQuestion CreateQuestion(TDomainQuestion question)
        {
            var questionData = _mapper.Map<TDataQuestion>(question);
            _repository.CreateQuestion(questionData);

            var questionResult = _mapper.Map<TDomainQuestion>(_repository.GetQuestion(questionData.Id));
            return questionResult;
        }

        public void DeleteQuestion(long id)
        {
            // TODO this way removes question without checking its type
            // It needs to check question type before delete
            _repository.DeleteQuestion(id, (short)_questionType);
        }

        public void EditQuestion(TDomainQuestion question)
        {
            var questionData = _mapper.Map<TDataQuestion>(question);
            _repository.EditQuestion(questionData);
        }

        public TDomainQuestion GetQuestion(long id)
        {
            return _mapper.Map<TDomainQuestion>(_repository.GetQuestion(id, (short)_questionType));
        }
    }
}
