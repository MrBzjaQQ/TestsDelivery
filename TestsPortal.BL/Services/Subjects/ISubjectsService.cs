using TestsPortal.Domain.Subjects;

namespace TestsPortal.BL.Services.Subjects
{
    public interface ISubjectsService
    {
        Subject CreateSubject(Subject subject);

        IEnumerable<Subject> CreateSubjects(IEnumerable<Subject> subject);
    }
}
