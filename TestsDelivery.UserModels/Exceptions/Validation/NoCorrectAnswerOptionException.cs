namespace TestsDelivery.UserModels.Exceptions.Validation
{
    public class NoCorrectAnswerOptionException : QuestionValidationException
    {
        public NoCorrectAnswerOptionException()
            : base("No correct option found.")
        {
        }
    }
}
