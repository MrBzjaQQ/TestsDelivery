using AutoMapper;
using TestsDelivery.UserModels.Questions.MultipleChoice;
using TestsDelivery.BL.Services.Questions.MultipleChoice;
using TestsDelivery.UserModels.Validators.Questions;
using TestsDelivery.Domain.Questions;

namespace TestsDelivery.BL.Mediators.Questions.MultipleChoice
{
    public class McqMediator : BaseMediator<McqCreateModel, McqEditModel, McqReadModel, MultipleChoiceQuestion>, IMcqMediator
    {
        private readonly IMcqModelValidator _validator;

        public McqMediator(IMcqService service, IMcqModelValidator validator, IMapper mapper)
            : base(service, mapper)
        {
            _validator = validator;
        }

        public override McqReadModel CreateQuestion(McqCreateModel model)
        {
            _validator.ValidateCreateModel(model);
            return base.CreateQuestion(model);
        }

        public override void EditQuestion(McqEditModel model)
        {
            _validator.ValidateEditModel(model);
            base.EditQuestion(model);
        }
    }
}
