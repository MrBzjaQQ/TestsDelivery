using System.Collections.Generic;
using TestsDelivery.UserModels.ListFilters;
using TestsDelivery.UserModels.Subject;

namespace TestsDelivery.BL.Mediators.Subjects
{
    public interface ISubjectsMediator
    {
        SubjectReadModel CreateSubject(SubjectCreateModel model);

        SubjectReadModel GetSubject(long id);

        void EditSubject(SubjectEditModel model);

        SubjectsListModel GetList(ListFilterModel listModel);
    }
}
