using System.Linq.Expressions;

namespace TestsDelivery.DAL.Shared
{
    public interface ISpecification<TEntity>
    {
        ISpecification<TEntity> And(Specification<TEntity> specification);

        ISpecification<TEntity> And(Expression<Func<TEntity, bool>> predicate);

        ISpecification<TEntity> Or(Specification<TEntity> specification);

        ISpecification<TEntity> Or(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> SatisfyingEntitiesFrom(IQueryable<TEntity> entities);
    }
}
