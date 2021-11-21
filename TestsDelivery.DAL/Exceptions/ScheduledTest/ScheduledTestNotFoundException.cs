namespace TestsDelivery.DAL.Exceptions.ScheduledTest
{
    public class ScheduledTestNotFoundException : ScheduledTestException
    {
        public ScheduledTestNotFoundException(long id)
            : base($"Scheduled test with id = {id} is not found.")
        {
        }
    }
}
