namespace TestsDelivery.DAL.Shared
{
    public interface IFilterBuilderBase<TEntity>
    {
        IFilterBuilderBase<TEntity> Take(int take);

        IFilterBuilderBase<TEntity> Skip(int skip);

        GenericFilter<TEntity> Build();
    }
}
