using AutoMapper;
using TestsDelivery.BL.Services.Marking.Essay;
using TestsDelivery.Domain.Marking;
using TestsDelivery.UserModels.Marking.Questions;

namespace TestsDelivery.BL.Mediators.Marking.Essay
{
    public class EssayMarkMediator : MarkMediatorBase<EssayMarkCreateModel, EssayMarkReadModel, MarkedEssay>, IEssayMarkMediator
    {
        public EssayMarkMediator(IEssayMarkService markService, IMapper mapper)
            : base(markService, mapper)
        {
        }
    }
}
