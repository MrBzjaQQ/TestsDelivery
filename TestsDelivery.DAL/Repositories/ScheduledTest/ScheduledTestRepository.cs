using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Exceptions.ScheduledTest;

namespace TestsDelivery.DAL.Repositories.ScheduledTest
{
    public class ScheduledTestRepository : BaseRepository<Models.ScheduledTest.ScheduledTest>, IScheduledTestRepository
    {
        public ScheduledTestRepository(TestsDeliveryContext context)
            : base(context)
        {
        }
    }
}
