using System;

namespace TestsDelivery.DAL.Exceptions.Subjects
{
    public class SubjectNotFoundException : Exception
    {
        public SubjectNotFoundException(int id)
            : base($"Subject with id = {id} does not exists.")
        {
        }
    }
}
