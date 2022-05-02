using TestsDelivery.DAL.Shared;

namespace TestsPortal.DAL.Repositories
{
    public interface IBaseRepository<TEntity>
        where TEntity : class
    {
        TEntity GetById(long id);

        void Create(TEntity entity);

        void Create(IEnumerable<TEntity> entities);

        void Update(TEntity entity);

        void Update(IEnumerable<TEntity> entities);

        void Delete(long id);

        IList<TEntity> GetByFilter(GenericFilter<TEntity> filter);
    }
}
