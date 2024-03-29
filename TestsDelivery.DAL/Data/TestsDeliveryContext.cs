﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestsDelivery.DAL.Models.Candidate;
using TestsDelivery.DAL.Models.Identity;
using TestsDelivery.DAL.Models.Marking;
using TestsDelivery.DAL.Models.Questions;
using TestsDelivery.DAL.Models.ScheduledTest;
using TestsDelivery.DAL.Models.Subject;
using TestsDelivery.DAL.Models.Test;

namespace TestsDelivery.DAL.Data
{
    /// <summary>
    /// TestsDeliveryContext represents database context
    /// </summary>
    public class TestsDeliveryContext : IdentityDbContext<User>
    {
        public TestsDeliveryContext(DbContextOptions<TestsDeliveryContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ChoiceAnswer>()
                .HasOne(x => x.AnswerOption)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<AnswerOption> AnswerOptions { get; set; }

        public DbSet<Candidate> Candidates { get; set; }

        public DbSet<QuestionInTest> QuestionInTests { get; set; }

        public DbSet<Test> Tests { get; set; }

        public DbSet<ScheduledTest> ScheduledTests { get; set; }

        public DbSet<ScheduledTestInstance> ScheduledTestInstances { get; set; }

        public DbSet<TextAnswer> TextAnswers { get; set; }

        public DbSet<ChoiceAnswer> ChoiceAnswers { get; set; }

        public DbSet<EssayMark> EssayMarks { get; set; }

        public DbSet<ChoiceMark> ChoiceMarks { get; set; }
    }
}