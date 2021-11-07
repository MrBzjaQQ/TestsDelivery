namespace TestsDelivery.DAL.Exceptions.Questions
{
    public class QuestionNotFoundException : QuestionException
    {
        public QuestionNotFoundException()
            : base("Question not found")
        {
        }

        public QuestionNotFoundException(long id)
            : base($"Question with id = {id} is not found")
        {
        }
    }
}
