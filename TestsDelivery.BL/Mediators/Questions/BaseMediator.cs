using AutoMapper;
using TestsDelivery.BL.Models.Questions.BaseQuestion;
using TestsDelivery.BL.Services.Questions;
using TestsDelivery.Domain.Questions;

namespace TestsDelivery.BL.Mediators.Questions
{
    public abstract class BaseMediator<TCreateModel, TEditModel, TReadModel, TDomainModel> : IBaseMediator<TCreateModel, TEditModel, TReadModel>
        where TCreateModel : BaseQuestionCreateModel
        where TEditModel: BaseQuestionEditModel
        where TReadModel: BaseQuestionReadModel
        where TDomainModel: QuestionBase
    {
        private readonly IBaseQuestionService<TDomainModel> _service;
        private readonly IMapper _mapper;

        protected BaseMediator(
            IBaseQuestionService<TDomainModel> service,
            IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public virtual TReadModel CreateQuestion(TCreateModel model)
        {
            var question = _mapper.Map<TDomainModel>(model);
            var createdQuestion = _service.CreateQuestion(question);
            return _mapper.Map<TReadModel>(createdQuestion);
        }

        public void DeleteQuestion(long id)
        {
            _service.DeleteQuestion(id);
        }

        public virtual void EditQuestion(TEditModel model)
        {
            var question = _mapper.Map<TDomainModel>(model);
            _service.EditQuestion(question);
        }

        public virtual TReadModel GetQuestion(long id)
        {
            return _mapper.Map<TReadModel>(_service.GetQuestion(id));
        }
    }
}
