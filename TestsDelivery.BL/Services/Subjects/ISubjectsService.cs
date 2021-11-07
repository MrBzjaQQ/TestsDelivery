using TestsDelivery.Domain.Subject;

namespace TestsDelivery.BL.Services.Subjects
{
    public interface ISubjectsService
    {
        Subject CreateSubject(Subject subject);

        Subject GetSubject(long id);

        void EditSubject(Subject subject);
    }
}
