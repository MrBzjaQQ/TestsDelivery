using AutoMapper;
using TestsDelivery.UserModels.Questions.Essay;
using TestsDelivery.BL.Services.Questions.Essay;
using TestsDelivery.BL.Validators.Questions;
using TestsDelivery.Domain.Questions;

namespace TestsDelivery.BL.Mediators.Questions.Essay
{
    public class EssayMediator : BaseMediator<EssayCreateModel, EssayEditModel, EssayReadModel, EssayQuestion>, IEssayMediator
    {
        private readonly IEssayValidator _validator;

        public EssayMediator(
            IEssayService service,
            IEssayValidator validator,
            IMapper mapper)
            : base(service, mapper)
        {
            _validator = validator;
        }

        public override EssayReadModel CreateQuestion(EssayCreateModel model)
        {
            _validator.ValidateCreateModel(model);
            return base.CreateQuestion(model);
        }

        public override void EditQuestion(EssayEditModel model)
        {
            _validator.ValidateEditModel(model);
            base.EditQuestion(model);
        }
    }
}
