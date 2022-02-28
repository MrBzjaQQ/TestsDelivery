using TestDomainModel = TestsDelivery.Domain.Test.Test;

namespace TestsDelivery.BL.Services.Test
{
    public interface ITestService
    {
        TestDomainModel CreateTest(TestDomainModel test);

        TestDomainModel GetTest(long id);

        TestDomainModel GetFullTest(long id);

        void EditTest(TestDomainModel test);
    }
}
