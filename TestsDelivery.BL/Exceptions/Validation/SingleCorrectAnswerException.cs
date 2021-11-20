using System;

namespace TestsDelivery.BL.Exceptions.Validation
{
    public class SingleCorrectAnswerException : QuestionValidationException
    {
        public SingleCorrectAnswerException()
            : base("Only one correct answer found for multiple choice question")
        {
        }
    }
}
