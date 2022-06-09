using AutoMapper;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Models.Marking;

namespace TestsDelivery.DAL.Repositories.Marking.Choice
{
    public class ChoiceMarkingRepository : MarkingRepositoryBase<ChoiceMark>, IChoiceMarkingRepository
    {
        public ChoiceMarkingRepository(TestsDeliveryContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
