using System.Collections.Generic;
using TestsDelivery.UserModels.Lists;
using TestsDelivery.UserModels.Subject;

namespace TestsDelivery.BL.Mediators.Subjects
{
    public interface ISubjectsMediator
    {
        SubjectReadModel CreateSubject(SubjectCreateModel model);

        SubjectReadModel GetSubject(long id);

        void EditSubject(SubjectEditModel model);

        IEnumerable<SubjectInListModel> GetList(ListModel listModel);
    }
}
