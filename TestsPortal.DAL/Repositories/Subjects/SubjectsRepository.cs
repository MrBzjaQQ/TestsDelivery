using TestsPortal.DAL.Data;
using TestsPortal.DAL.Models.Subject;

namespace TestsPortal.DAL.Repositories.Subjects
{
    public class SubjectsRepository : ISubjectsRepository
    {
        private readonly TestsPortalContext _context;

        public SubjectsRepository(TestsPortalContext context)
        {
            _context = context;
        }

        public void CreateSubject(Subject subject)
        {
            _context.Subjects.Add(subject);
            _context.SaveChanges();
        }
    }
}
