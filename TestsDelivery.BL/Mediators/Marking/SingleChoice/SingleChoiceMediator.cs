using AutoMapper;
using TestsDelivery.BL.Services.Marking;
using TestsDelivery.DAL.Models.Marking;
using TestsDelivery.UserModels.Marking.Questions;

namespace TestsDelivery.BL.Mediators.Marking.SingleChoice
{
    public class SingleChoiceMediator : MarkMediatorBase<ScqMarkCreateModel, ScqMarkReadModel, ChoiceMark>, ISingleChoiceMediator
    {
        public SingleChoiceMediator(IMarkServiceBase<ChoiceMark> markService, IMapper mapper)
            : base(markService, mapper)
        {
        }
    }
}
