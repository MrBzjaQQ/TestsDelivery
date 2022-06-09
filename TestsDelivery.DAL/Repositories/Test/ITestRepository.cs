using TestsDelivery.DAL.Shared.Repository;
using TestData = TestsDelivery.DAL.Models.Test.Test;

namespace TestsDelivery.DAL.Repositories.Test
{
    public interface ITestRepository : IBaseRepository<TestData>
    {
    }
}
