using Microsoft.EntityFrameworkCore;
using TestsPortal.DAL.Models.Candidate;
using TestsPortal.DAL.Models.Questions;
using TestsPortal.DAL.Models.ScheduledTests;
using TestsPortal.DAL.Models.Subject;
using TestsPortal.DAL.Models.Tests;

namespace TestsPortal.DAL.Data
{
    public class TestsPortalContext : DbContext
    {
        public TestsPortalContext(DbContextOptions<TestsPortalContext> options)
            : base(options)
        {
        }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<AnswerOption> AnswerOptions { get; set; }

        public DbSet<Candidate> Candidates { get; set; }

        public DbSet<QuestionInTest> QuestionInTests { get; set; }

        public DbSet<Test> Tests { get; set; }

        public DbSet<ScheduledTest> ScheduledTests { get; set; }
    }
}
