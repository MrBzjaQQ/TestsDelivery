namespace TestsDelivery.DAL.Exceptions.Questions
{
    public class QuestionIncorrectTypeException : QuestionException
    {
        public QuestionIncorrectTypeException(short expected, short actual)
            : base($"Question type does not match. Expected {expected}, but found {actual}")
        {
        }
    }
}
