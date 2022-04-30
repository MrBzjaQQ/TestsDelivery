using TestsDelivery.DAL.Shared;

namespace TestsPortal.DAL.Repositories
{
    public interface IBaseRepository<TEntity>
        where TEntity : class
    {
        TEntity GetById(long id);

        void Create(TEntity entity);

        void Update(TEntity entity);

        IList<TEntity> GetByFilter(GenericFilter<TEntity> filter);
    }
}
