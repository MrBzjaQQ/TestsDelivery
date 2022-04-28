using System;
using System.Linq;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Exceptions.Test;
using TestData = TestsDelivery.DAL.Models.Test.Test;

namespace TestsDelivery.DAL.Repositories.Test
{
    public class TestRepository : BaseRepository<TestData>, ITestRepository
    {
        public TestRepository(TestsDeliveryContext context)
            : base(context)
        {
        }
    }
}
