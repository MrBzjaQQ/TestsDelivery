using System.ComponentModel.DataAnnotations.Schema;
using TestsDelivery.DAL.Shared.Models;

namespace TestsDelivery.DAL.Models.Questions
{
    public record AnswerBase : IdEntity<long>
    {
        [ForeignKey(nameof(Questions.Question))]
        public long QuestionId { get; set; }

        [ForeignKey(nameof(Models.ScheduledTest.ScheduledTest))]
        public long ScheduledTestId { get; set; }

        [ForeignKey(nameof(Models.Candidate.Candidate))]
        public long CandidateId { get; set; }

        public virtual Question Question { get; set; }

        public virtual ScheduledTest.ScheduledTest ScheduledTest { get; set; }

        public virtual Candidate.Candidate Candidate { get; set; }
    }
}
