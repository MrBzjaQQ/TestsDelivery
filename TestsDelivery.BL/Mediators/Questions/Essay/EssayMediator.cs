using AutoMapper;
using TestsDelivery.BL.Models.Questions.Essay;
using TestsDelivery.BL.Services.Questions.Essay;
using TestsDelivery.BL.Validators.Questions;
using TestsDelivery.Domain.Questions;

namespace TestsDelivery.BL.Mediators.Questions.Essay
{
    public class EssayMediator : IEssayMediator
    {
        private readonly IEssayService _service;
        private readonly IEssayValidator _validator;
        private readonly IMapper _mapper;

        public EssayMediator(IEssayService service, IEssayValidator validator, IMapper mapper)
        {
            _validator = validator;
            _service = service;
            _mapper = mapper;
        }

        public EssayReadModel CreateQuestion(EssayCreateModel model)
        {
            _validator.ValidateCreateModel(model);
            var question = _mapper.Map<EssayQuestion>(model);
            var createdQuestion = _service.CreateQuestion(question);
            return _mapper.Map<EssayReadModel>(createdQuestion);
        }

        public void EditQuestion(EssayEditModel model)
        {
            _validator.ValidateEditModel(model);
            var question = _mapper.Map<EssayQuestion>(model);
            _service.EditQuestion(question);
        }

        public EssayReadModel GetQuestion(long id)
        {
            return _mapper.Map<EssayReadModel>(_service.GetQuestion(id));
        }
    }
}
