using AutoMapper;
using TestsDelivery.BL.Models.Questions.SingleChoice;
using TestsDelivery.BL.Services.Questions.SingleChoice;
using TestsDelivery.Domain.Questions;

namespace TestsDelivery.BL.Mediators.Questions
{
    public class ScqMediator: IScqMediator
    {
        private readonly IScqService _service;
        private readonly IMapper _mapper;

        public ScqMediator(
            IScqService service,
            IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public ScqReadModel CreateQuestion(ScqCreateModel model)
        {
            var question = _mapper.Map<SingleChoiceQuestion>(model);
            var createdQuestion = _service.CreateQuestion(question);
            return _mapper.Map<ScqReadModel>(createdQuestion);
        }

        public void EditQuestion(ScqEditModel model)
        {
            var question = _mapper.Map<SingleChoiceQuestion>(model);
            _service.EditQuestion(question);
        }

        public ScqReadModel GetQuestion(long id)
        {
            return _mapper.Map<ScqReadModel>(_service.GetQuestion(id));
        }
    }
}
