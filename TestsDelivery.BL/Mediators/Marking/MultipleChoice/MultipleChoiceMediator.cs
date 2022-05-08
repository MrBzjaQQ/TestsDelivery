using AutoMapper;
using TestsDelivery.BL.Services.Marking;
using TestsDelivery.DAL.Models.Marking;
using TestsDelivery.UserModels.Marking.Questions;

namespace TestsDelivery.BL.Mediators.Marking.MultipleChoice
{
    public class MultipleChoiceMediator : MarkMediatorBase<McqMarkCreateModel, McqMarkReadModel, ChoiceMark>, IMultipleChoiceMediator
    {
        public MultipleChoiceMediator(IMarkServiceBase<ChoiceMark> markService, IMapper mapper)
            : base(markService, mapper)
        {
        }
    }
}
