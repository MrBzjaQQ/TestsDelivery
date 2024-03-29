﻿using System.ComponentModel.DataAnnotations.Schema;
using TestsDelivery.DAL.Shared.Models;
using TestsPortal.DAL.Models.Candidates;
using TestsPortal.DAL.Models.ScheduledTests;

namespace TestsPortal.DAL.Models.Questions
{
    public record AnswerBase : IdEntity<long>
    {
        [ForeignKey(nameof(Questions.Question))]
        public long QuestionId { get; set; }

        [ForeignKey(nameof(ScheduledTests.ScheduledTest))]
        public long ScheduledTestId { get; set; }

        [ForeignKey(nameof(Candidates.Candidate))]
        public long CandidateId { get; set; }

        public virtual Question Question { get; set; }

        public virtual ScheduledTest ScheduledTest { get; set; }

        public virtual Candidate Candidate { get; set; }
    }
}
