using AutoMapper;
using TestsDelivery.BL.Models.Questions.MultipleChoice;
using TestsDelivery.BL.Services.Questions.MultipleChoice;
using TestsDelivery.BL.Validators.Questions;
using TestsDelivery.Domain.Questions;

namespace TestsDelivery.BL.Mediators.Questions.MultipleChoice
{
    public class McqMediator : IMcqMediator
    {
        private readonly IMcqService _service;
        private readonly IMcqModelValidator _validator;
        private readonly IMapper _mapper;

        public McqMediator(IMcqService service, IMcqModelValidator validator, IMapper mapper)
        {
            _service = service;
            _validator = validator;
            _mapper = mapper;
        }

        public McqReadModel CreateQuestion(McqCreateModel model)
        {
            _validator.ValidateCreateModel(model);
            var question = _mapper.Map<MultipleChoiceQuestion>(model);
            var createdQuestion = _service.CreateQuestion(question);
            return _mapper.Map<McqReadModel>(createdQuestion);
        }

        public void EditQuestion(McqEditModel model)
        {
            _validator.ValidateEditModel(model);
            var question = _mapper.Map<MultipleChoiceQuestion>(model);
            _service.EditQuestion(question);
        }

        public McqReadModel GetQuestion(long id)
        {
            return _mapper.Map<McqReadModel>(_service.GetQuestion(id));
        }
    }
}
