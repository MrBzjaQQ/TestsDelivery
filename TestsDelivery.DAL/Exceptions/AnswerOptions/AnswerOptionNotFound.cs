namespace TestsDelivery.DAL.Exceptions.AnswerOptions
{
    public class AnswerOptionNotFound : AnswerOptionException
    {
        public AnswerOptionNotFound(long id)
            : base($"Answer option with id = {id} is not found.")
        {
        }
    }
}
