using AutoMapper;
using TestsDelivery.BL.Services.Marking.SingleChoice;
using TestsDelivery.Domain.Marking;
using TestsDelivery.UserModels.Marking.Questions;

namespace TestsDelivery.BL.Mediators.Marking.SingleChoice
{
    public class SingleChoiceMarkMediator : MarkMediatorBase<ScqMarkCreateModel, ScqMarkReadModel, MarkedSingleChoice>, ISingleChoiceMarkMediator
    {
        public SingleChoiceMarkMediator(ISingleChoiceMarkService markService, IMapper mapper)
            : base(markService, mapper)
        {
        }
    }
}
