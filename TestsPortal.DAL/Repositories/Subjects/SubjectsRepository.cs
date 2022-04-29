using TestsPortal.DAL.Data;
using TestsPortal.DAL.Models.Subject;

namespace TestsPortal.DAL.Repositories.Subjects
{
    public class SubjectsRepository : BaseRepository<Subject>, ISubjectsRepository
    {
        public SubjectsRepository(TestsPortalContext context)
            : base(context)
        {
        }

        public void Create(IEnumerable<Subject> subjects)
        {
            Context.Subjects.AddRange(subjects);
            Context.SaveChanges();
        }
    }
}
