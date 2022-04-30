namespace TestsDelivery.DAL.Shared
{
    public interface ISorting<TEntity>
    {
        IQueryable<TEntity> ApplySorting(IQueryable<TEntity> entities);
    }
}
