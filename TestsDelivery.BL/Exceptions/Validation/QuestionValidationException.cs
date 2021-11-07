using System;

namespace TestsDelivery.BL.Exceptions.Validation
{
    public class QuestionValidationException : Exception
    {
        public QuestionValidationException(string message)
            : base(message)
        {
        }
    }
}
