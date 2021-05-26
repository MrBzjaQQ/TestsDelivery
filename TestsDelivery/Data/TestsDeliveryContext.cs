using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestsDelivery.Models.Identity;

namespace TestsDelivery.Data
{
    public class TestsDeliveryContext: IdentityDbContext<User>
    {
        public TestsDeliveryContext(DbContextOptions<TestsDeliveryContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}