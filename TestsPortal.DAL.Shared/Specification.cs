using System.Linq.Expressions;

namespace TestsDelivery.DAL.Shared
{
    public class Specification<TEntity> : ISpecification<TEntity>
    {
        public Expression<Func<TEntity, bool>> Predicate { get; set; }

        public Specification(Expression<Func<TEntity, bool>> predicate)
        {
            Predicate = predicate;
        }

        public ISpecification<TEntity> And(Specification<TEntity> specification)
        {
            if (specification == null)
                throw new ArgumentNullException(nameof(specification));

            var parameter = Predicate.Parameters[0];
            var body = Expression.AndAlso(Predicate.Body, specification.Predicate.Body.ReplaceParameter(specification.Predicate.Parameters[0], parameter));
            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, Predicate.Parameters[0]);
            return new Specification<TEntity>(lambda);
        }

        public ISpecification<TEntity> And(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var parameter = Predicate.Parameters[0];
            var body = Expression.AndAlso(Predicate.Body, predicate.Body.ReplaceParameter(predicate.Parameters[0], parameter));
            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, Predicate.Parameters[0]);
            return new Specification<TEntity>(lambda);
        }

        public ISpecification<TEntity> Or(Specification<TEntity> specification)
        {
            if (specification == null)
                throw new ArgumentNullException(nameof(specification));

            var parameter = Predicate.Parameters[0];
            var body = Expression.AndAlso(Predicate.Body, specification.Predicate.Body.ReplaceParameter(specification.Predicate.Parameters[0], parameter));
            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, Predicate.Parameters[0]);
            return new Specification<TEntity>(lambda);
        }

        public ISpecification<TEntity> Or(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var parameter = Predicate.Parameters[0];
            var body = Expression.AndAlso(Predicate.Body, predicate.Body.ReplaceParameter(predicate.Parameters[0], parameter));
            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, Predicate.Parameters[0]);
            return new Specification<TEntity>(lambda);
        }

        public IQueryable<TEntity> SatisfyingEntitiesFrom(IQueryable<TEntity> entities)
        {
            if (Predicate != null)
                return entities.Where(Predicate);

            return entities;
        }
    }
}
