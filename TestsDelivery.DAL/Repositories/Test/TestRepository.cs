using System;
using System.Linq;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Exceptions.Test;
using TestData = TestsDelivery.DAL.Models.Test.Test;

namespace TestsDelivery.DAL.Repositories.Test
{
    public class TestRepository : ITestRepository
    {
        private readonly TestsDeliveryContext _context;

        public TestRepository(TestsDeliveryContext context)
        {
            _context = context;
        }

        public void CreateTest(TestData test)
        {
            _context.Tests.Add(test);
            _context.SaveChanges();
        }

        public void EditTest(TestData test)
        {
            _context.Tests.Update(test);
            _context.SaveChanges();
        }

        public TestData GetTest(long id)
        {
            try
            {
                return _context.Tests.Single(t => t.Id == id);
            }
            catch (InvalidOperationException)
            {
                throw new TestNotFoundException(id);
            }
        }
    }
}
