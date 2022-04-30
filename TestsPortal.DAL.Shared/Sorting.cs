namespace TestsDelivery.DAL.Shared
{
    public abstract class Sorting<TEntity> : ISorting<TEntity>
    {
        protected Func<IQueryable<TEntity>, IQueryable<TEntity>> OrderByFilter { get; set; }

        public IQueryable<TEntity> ApplySorting(IQueryable<TEntity> entities)
        {
            if (OrderByFilter == null)
                return entities;

            return OrderByFilter.Invoke(entities);
        }
    }
}
