﻿namespace TestsDelivery.UserModels.Exceptions.Validation
{
    public class MultipleCorrectAnswersException : QuestionValidationException
    {
        public MultipleCorrectAnswersException()
            : base("Multiple correct answers found for single choice question")
        {
        }
    }
}
