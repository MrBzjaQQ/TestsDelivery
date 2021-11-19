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

        protected BaseQuestionService(IQuestionsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public TDomainQuestion CreateQuestion(TDomainQuestion question)
        {
            var questionData = _mapper.Map<TDataQuestion>(question);
            _repository.CreateQuestion(questionData);

            var questionResult = _mapper.Map<TDomainQuestion>(question);
            return questionResult;
        }

        public void EditQuestion(TDomainQuestion question)
        {
            var questionData = _mapper.Map<TDataQuestion>(question);
            _repository.EditQuestion(questionData);
        }

        public TDomainQuestion GetQuestion(long id)
        {
            return _mapper.Map<TDomainQuestion>(_repository.GetQuestion(id));
        }
    }
}
