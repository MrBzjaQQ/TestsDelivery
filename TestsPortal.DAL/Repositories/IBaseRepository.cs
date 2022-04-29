using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsPortal.DAL.Repositories
{
    public interface IBaseRepository<TEntity>
        where TEntity : class
    {
        TEntity GetById(long id);

        void Create(TEntity entity);

        void Update(TEntity entity);
    }
}
