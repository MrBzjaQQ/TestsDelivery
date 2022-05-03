namespace TestsDelivery.DAL.Shared.Repository
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

        IEnumerable<TEntity> GetByFilter(GenericFilter<TEntity> filter);

        IEnumerable<TProjection> GetWithProjection<TProjection>(GenericFilter<TEntity> filter);
    }
}
