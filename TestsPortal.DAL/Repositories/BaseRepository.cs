using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestsDelivery.DAL.Shared;
using TestsPortal.DAL.Data;
using TestsPortal.DAL.Models;

namespace TestsPortal.DAL.Repositories
{
    // TODO: generalize and move to TestsDelivery.DAL.Shared
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : IdOriginalIdEntity<long>
    {
        protected readonly TestsPortalContext Context;
        protected readonly IMapper Mapper;
        private readonly DbSet<TEntity> _dbSet;

        public BaseRepository(TestsPortalContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
            _dbSet = Context.Set<TEntity>();
        }

        public virtual void Create(TEntity entity)
        {
            _dbSet.Add(entity);
            Context.SaveChanges();
        }

        public virtual TEntity GetById(long id)
        {
            return _dbSet.Single(x => x.Id == id);
        }

        public virtual IList<TEntity> GetByFilter(GenericFilter<TEntity> filter)
        {
            return ApplyFilter(_dbSet, filter).ToList();
        }

        public virtual void Update(TEntity entity)
        {
            Context.Update(entity);
            Context.SaveChanges();
        }

        protected IQueryable<TEntityType> ApplyFilter<TEntityType>(IQueryable<TEntityType> query, GenericFilter<TEntityType> filter)
        {
            var targetQuery = query;
            targetQuery = ApplySpecification(targetQuery, filter);
            targetQuery = ApplySorting(targetQuery, filter);
            targetQuery = ApplyPaging(targetQuery, filter);
            return targetQuery;
        }

        protected IQueryable<TEntityType> ApplySpecification<TEntityType>(IQueryable<TEntityType> query, GenericFilter<TEntityType> filter)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            return filter.Specification.SatisfyingEntitiesFrom(query);
        }

        protected IQueryable<TEntityType> ApplySorting<TEntityType>(IQueryable<TEntityType> query, GenericFilter<TEntityType> filter)
        {
            if (filter?.Sorting == null)
                return query;

            return filter.Sorting.ApplySorting(query);
        }

        protected IQueryable<TEntityType> ApplyPaging<TEntityType>(IQueryable<TEntityType> query, GenericFilter<TEntityType> filter)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var targetQuery = query;

            if (filter == null)
                return query;

            if (filter.Take.HasValue)
                targetQuery = targetQuery.Take(filter.Take.Value);

            if (filter.Skip.HasValue)
                targetQuery = targetQuery.Skip(filter.Skip.Value);

            return targetQuery;
        }
    }
}
