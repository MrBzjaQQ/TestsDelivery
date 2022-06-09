using AutoMapper;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Models.Marking;

namespace TestsDelivery.DAL.Repositories.Marking.Essay
{
    public class EssayMarkingRepository : MarkingRepositoryBase<EssayMark>, IEssayMarkingRepository
    {
        public EssayMarkingRepository(TestsDeliveryContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
