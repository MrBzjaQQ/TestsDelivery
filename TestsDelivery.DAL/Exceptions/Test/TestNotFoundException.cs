namespace TestsDelivery.DAL.Exceptions.Test
{
    public class TestNotFoundException : TestException
    {
        public TestNotFoundException(long id)
            : base($"Test with id = {id} is not found.")
        {
        }
    }
}
