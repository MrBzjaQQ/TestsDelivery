namespace TestsDelivery.BL.Exceptions.Validation
{
    public class NoCorrectAnswerOptionException : QuestionValidationException
    {
        public NoCorrectAnswerOptionException()
            : base("No correct option found.")
        {
        }
    }
}
