using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestsDelivery.DAL.Shared.Models;

namespace TestsDelivery.DAL.Shared.Repository
{
    public abstract class BaseRepository<TDbContext, TEntity> : IBaseRepository<TEntity>
        where TDbContext : DbContext
        where TEntity : IdEntity<long>
    {
        protected readonly TDbContext Context;
        protected readonly IMapper Mapper;
        protected readonly DbSet<TEntity> DbSet;

        public BaseRepository(TDbContext context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
            DbSet = Context.Set<TEntity>();
        }

        public virtual void Create(TEntity entity)
        {
            DbSet.Add(entity);
            Context.SaveChanges();
        }

        public void Create(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
            Context.SaveChanges();
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
            Context.SaveChanges();
        }

        public void Delete(long id)
        {
            var entity = DbSet.Single(e => e.Id == id);
            DbSet.Remove(entity);
            Context.SaveChanges();
        }

        public virtual TEntity GetById(long id)
        {
            return DbSet.Single(x => x.Id == id);
        }

        public virtual IList<TEntity> GetByFilter(GenericFilter<TEntity> filter)
        {
            return ApplyFilter(DbSet, filter).ToList();
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
