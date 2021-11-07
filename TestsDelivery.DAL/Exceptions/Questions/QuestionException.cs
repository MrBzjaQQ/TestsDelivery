using System;

namespace TestsDelivery.DAL.Exceptions.Questions
{
    public class QuestionException: Exception
    {
        public QuestionException(string message)
            : base(message)
        {
        }
    }
}
