using System;

namespace TestsDelivery.DAL.Exceptions.Subjects
{
    public class SubjectNotFoundException : Exception
    {
        public SubjectNotFoundException(long id)
            : base($"Subject with id = {id} does not exists.")
        {
        }
    }
}
