using AutoMapper;
using TestsDelivery.DAL.Shared.Repository;
using TestsPortal.DAL.Data;
using TestsPortal.DAL.Models.Subject;

namespace TestsPortal.DAL.Repositories.Subjects
{
    public class SubjectsRepository : BaseRepository<TestsPortalContext, Subject>, ISubjectsRepository
    {
        public SubjectsRepository(TestsPortalContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public void Create(IEnumerable<Subject> subjects)
        {
            Context.Subjects.AddRange(subjects);
            Context.SaveChanges();
        }
    }
}
