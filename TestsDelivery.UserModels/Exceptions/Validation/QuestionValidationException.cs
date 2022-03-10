using System;

namespace TestsDelivery.UserModels.Exceptions.Validation
{
    public class QuestionValidationException : Exception
    {
        public QuestionValidationException(string message)
            : base(message)
        {
        }
    }
}
