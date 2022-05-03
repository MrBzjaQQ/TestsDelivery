using System.Collections.Generic;
using TestsDelivery.Domain.Lists;
using TestsDelivery.Domain.Subject;

namespace TestsDelivery.BL.Services.Subjects
{
    public interface ISubjectsService
    {
        Subject CreateSubject(Subject subject);

        Subject GetSubject(long id);

        void EditSubject(Subject subject);

        IEnumerable<SubjectInListDto> GetList(ListFilter filter);
    }
}
