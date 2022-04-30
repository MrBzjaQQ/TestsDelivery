using System.ComponentModel.DataAnnotations.Schema;
using TestsPortal.DAL.Models.ScheduledTests;

namespace TestsPortal.DAL.Models.Questions
{
    public record Answer : IdEntity<long>
    {
        [ForeignKey(nameof(Questions.AnswerOption))]
        public long AnswerId { get; set; }

        [ForeignKey(nameof(Questions.Question))]
        public long QuestionId { get; set; }

        [ForeignKey(nameof(ScheduledTests.ScheduledTest))]
        public long ScheduledTestId { get; set; }

        public virtual AnswerOption AnswerOption { get; set; }

        public virtual Question Question { get; set; }

        public virtual ScheduledTest ScheduledTest { get; set; }
    }
}
