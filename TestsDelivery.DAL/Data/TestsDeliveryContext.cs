﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestsDelivery.DAL.Models.Identity;

namespace TestsDelivery.DAL.Data
{
    /// <summary>
    /// TestsDeliveryContext represents database context
    /// </summary>
    public class TestsDeliveryContext: IdentityDbContext<User>
    {
        public TestsDeliveryContext(DbContextOptions<TestsDeliveryContext> options)
            : base(options)
        {
        }
    }
}