using TestsPortal.DAL.Data;
using TestsPortal.DAL.Models;

namespace TestsPortal.DAL.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : IdEntity<long>
    {
        protected readonly TestsPortalContext Context;

        public BaseRepository(TestsPortalContext context)
        {
            Context = context;
        }

        public virtual void Create(TEntity entity)
        {
            Context.Add(entity);
            Context.SaveChanges();
        }

        public virtual TEntity GetById(long id)
        {
            return Context.Find<TEntity>(id);
        }

        public virtual void Update(TEntity entity)
        {
            Context.Update(entity);
            Context.SaveChanges();
        }
    }
}
