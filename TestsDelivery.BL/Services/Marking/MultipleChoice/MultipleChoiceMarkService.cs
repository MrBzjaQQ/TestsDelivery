using AutoMapper;
using TestsDelivery.DAL.Models.Marking;
using TestsDelivery.DAL.Repositories.Marking.Choice;
using TestsDelivery.Domain.Marking;

namespace TestsDelivery.BL.Services.Marking.MultipleChoice
{
    public class MultipleChoiceMarkService : MarkServiceBase<MarkedMultipleChoice, ChoiceMark>, IMultipleChoiceMarkService
    {
        public MultipleChoiceMarkService(IChoiceMarkingRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
