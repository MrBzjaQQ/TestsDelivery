using TestsDelivery.BL.Models.Subject;

namespace TestsDelivery.BL.Mediators
{
    public interface ISubjectsMediator
    {
        SubjectReadModel CreateSubject(SubjectCreateModel model);

        SubjectReadModel GetSubject(int id);

        void EditSubject(SubjectEditModel model);
    }
}
