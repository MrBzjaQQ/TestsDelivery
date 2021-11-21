using System;

namespace TestsDelivery.DAL.Exceptions.ScheduledTest
{
    public class ScheduledTestException : Exception
    {
        public ScheduledTestException(string message)
            : base(message)
        {
        }
    }
}
