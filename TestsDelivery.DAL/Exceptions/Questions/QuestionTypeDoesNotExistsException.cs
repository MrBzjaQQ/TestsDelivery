namespace TestsDelivery.DAL.Exceptions.Questions
{
    public class QuestionTypeDoesNotExistsException : QuestionException
    {
        public QuestionTypeDoesNotExistsException(short questionType)
            : base($"Question type does not exists: {questionType}")
        {
        }
    }
}
