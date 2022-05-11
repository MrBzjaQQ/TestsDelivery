using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TestsDelivery.DAL.Data;
using TestsDelivery.DAL.Models.ScheduledTest;
using TestsDelivery.DAL.Shared;
using TestsDelivery.DAL.Shared.Repository;

namespace TestsDelivery.DAL.Repositories.ScheduledTest
{
    public class ScheduledTestRepository : BaseRepository<TestsDeliveryContext, Models.ScheduledTest.ScheduledTest>, IScheduledTestRepository
    {
        public ScheduledTestRepository(TestsDeliveryContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public IEnumerable<ScheduledTestInList> GetList(GenericFilter<ScheduledTestInList> filter)
        {
            return ApplyFilter(GetScheduledTestsInList(), filter).ToList();
        }

        public int GetScheduledTestsCount(GenericFilter<ScheduledTestInList> filter)
        {
            return ApplySpecification(GetScheduledTestsInList(), filter).Count();
        }

        private IQueryable<ScheduledTestInList> GetScheduledTestsInList()
        {
            return Context.ScheduledTestInstances
                .Include(x => x.Candidate)
                .Join(Context.ScheduledTests.Include(x => x.Test),
                instance => instance.ScheduledTestId,
                test => test.Id,
                (instance, test) => new ScheduledTestInList
                {
                    Id = instance.Id,
                    Candidate = instance.Candidate,
                    Duration = test.Duration,
                    ExpirationDateTime = test.ExpirationDateTime,
                    Keycode = instance.Keycode,
                    Pin = instance.Pin,
                    StartDateTime = test.StartDateTime,
                    TestName = test.Test.Name
                });
        }
    }
}
