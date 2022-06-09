using System.Linq.Expressions;

namespace TestsDelivery.DAL.Shared
{
    public abstract class FilterBuilderBase<TEntity> : IFilterBuilderBase<TEntity>
    {
        private readonly GenericFilter<TEntity> _filter;

        public FilterBuilderBase()
        {
            _filter = new GenericFilter<TEntity>();
        }

        public IFilterBuilderBase<TEntity> Skip(int skip)
        {
            _filter.Skip = skip;
            return this;
        }

        public IFilterBuilderBase<TEntity> Take(int take)
        {
            _filter.Take = take;
            return this;
        }

        public GenericFilter<TEntity> Build()
        {
            return _filter;
        }

        protected void And(Expression<Func<TEntity, bool>> predicate)
        {
            _filter.AddAndSpecification(new Specification<TEntity>(predicate));
        }
    }
}
