using AutoMapper;
using TestsDelivery.DAL.Models.Marking;
using TestsDelivery.DAL.Repositories.Marking;
using TestsDelivery.Domain.Marking;

namespace TestsDelivery.BL.Services.Marking.Essay
{
    public class EssayMarkService : MarkServiceBase<MarkedEssay, EssayMark>, IEssayMarkService
    {
        public EssayMarkService(IMarkingRepositoryBase<EssayMark> repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
