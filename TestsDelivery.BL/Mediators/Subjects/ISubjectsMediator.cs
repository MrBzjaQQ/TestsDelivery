using TestsDelivery.BL.Models.Subject;

namespace TestsDelivery.BL.Mediators.Subjects
{
    public interface ISubjectsMediator
    {
        SubjectReadModel CreateSubject(SubjectCreateModel model);

        SubjectReadModel GetSubject(long id);

        void EditSubject(SubjectEditModel model);
    }
}
