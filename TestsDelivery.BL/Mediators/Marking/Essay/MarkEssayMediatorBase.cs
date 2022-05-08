using AutoMapper;
using TestsDelivery.BL.Services.Marking;
using TestsDelivery.Domain.Marking;
using TestsDelivery.UserModels.Marking.Questions;

namespace TestsDelivery.BL.Mediators.Marking.Essay
{
    public class MarkEssayMediatorBase : MarkMediatorBase<EssayMarkCreateModel, EssayMarkReadModel, MarkedEssay>, IMarkEssayMedaitorBase
    {
        public MarkEssayMediatorBase(IMarkServiceBase<MarkedEssay> markService, IMapper mapper)
            : base(markService, mapper)
        {
        }
    }
}
