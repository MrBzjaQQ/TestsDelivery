using TestsDelivery.DAL.Shared.Repository;

namespace TestsDelivery.DAL.Repositories.Marking
{
    public interface IMarkingRepositoryBase<TMark> : IBaseRepository<TMark>
        where TMark : class
    {
        TMark GetMark(long questionId, long testId);
    }
}
