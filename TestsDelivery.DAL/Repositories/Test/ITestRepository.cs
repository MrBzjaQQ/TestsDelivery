using TestData = TestsDelivery.DAL.Models.Test.Test;

namespace TestsDelivery.DAL.Repositories.Test
{
    public interface ITestRepository
    {
        void CreateTest(TestData test);

        void EditTest(TestData test);

        TestData GetTest(long id);
    }
}
