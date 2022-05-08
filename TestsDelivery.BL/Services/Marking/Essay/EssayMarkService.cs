using AutoMapper;
using TestsDelivery.DAL.Models.Marking;
using TestsDelivery.DAL.Repositories.Marking.Essay;
using TestsDelivery.Domain.Marking;

namespace TestsDelivery.BL.Services.Marking.Essay
{
    public class EssayMarkService : MarkServiceBase<MarkedEssay, EssayMark>, IEssayMarkService
    {
        public EssayMarkService(IEssayMarkingRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
