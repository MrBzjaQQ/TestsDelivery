using System.ComponentModel.DataAnnotations.Schema;
using TestsPortal.DAL.Models.Candidates;
using TestsPortal.DAL.Models.ScheduledTests;

namespace TestsPortal.DAL.Models.Questions
{
    public record TextAnswer : IdEntity<long>
    {
        public string Text { get; set; }

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
