using AutoMapper;
using TestsDelivery.BL.Services.Marking.MultipleChoice;
using TestsDelivery.Domain.Marking;
using TestsDelivery.UserModels.Marking.Questions;

namespace TestsDelivery.BL.Mediators.Marking.MultipleChoice
{
    public class MultipleChoiceMarkMediator : MarkMediatorBase<McqMarkCreateModel, McqMarkReadModel, MarkedMultipleChoice>, IMultipleChoiceMarkMediator
    {
        public MultipleChoiceMarkMediator(IMultipleChoiceMarkService markService, IMapper mapper)
            : base(markService, mapper)
        {
        }
    }
}
