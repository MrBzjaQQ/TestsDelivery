using TestsDelivery.DAL.Shared.Repository;
using TestsPortal.DAL.Models.Subject;

namespace TestsPortal.DAL.Repositories.Subjects
{
    public interface ISubjectsRepository : IBaseRepository<Subject>
    {
        void Create(IEnumerable<Subject> subjects);
    }
}
