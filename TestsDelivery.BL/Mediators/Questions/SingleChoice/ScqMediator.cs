using AutoMapper;
using TestsDelivery.BL.Models.Questions.SingleChoice;
using TestsDelivery.BL.Services.Questions.SingleChoice;
using TestsDelivery.BL.Validators.Questions;
using TestsDelivery.Domain.Questions;

namespace TestsDelivery.BL.Mediators.Questions.SingleChoice
{
    public class ScqMediator : BaseMediator<ScqCreateModel, ScqEditModel, ScqReadModel, SingleChoiceQuestion>, IScqMediator
    {
        private readonly IScqModelValidator _validator;

        public ScqMediator(
            IScqService service,
            IScqModelValidator validator,
            IMapper mapper)
            : base(service, mapper)
        {
            _validator = validator;
        }

        public override ScqReadModel CreateQuestion(ScqCreateModel model)
        {
            _validator.ValidateCreateModel(model);
            return base.CreateQuestion(model);
        }

        public override void EditQuestion(ScqEditModel model)
        {
            _validator.ValidateEditModel(model);
            base.EditQuestion(model);
        }
    }
}
