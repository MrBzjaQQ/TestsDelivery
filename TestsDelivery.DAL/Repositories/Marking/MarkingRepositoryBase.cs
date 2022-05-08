using AutoMapper;
using System.Linq;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Models.Marking;
using TestsDelivery.DAL.Shared.Repository;

namespace TestsDelivery.DAL.Repositories.Marking
{
    public class MarkingRepositoryBase<TMark> : BaseRepository<TestsDeliveryContext, TMark>, IMarkingRepositoryBase<TMark>
        where TMark : MarkBase
    {
        public MarkingRepositoryBase(TestsDeliveryContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public TMark GetMark(long questionId, long testId)
        {
            return DbSet.Single(x => x.QuestionId == questionId && x.TestInstanceId == testId);
        }
    }
}
