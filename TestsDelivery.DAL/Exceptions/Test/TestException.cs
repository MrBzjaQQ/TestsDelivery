using System;

namespace TestsDelivery.DAL.Exceptions.Test
{
    public class TestException : Exception
    {
        public TestException(string message)
            : base(message)
        {
        }
    }
}
