using System.ComponentModel.DataAnnotations.Schema;
using TestsPortal.DAL.Models.Questions;

namespace TestsPortal.DAL.Models.Tests
{
    public record QuestionInTest : IdOriginalIdEntity<long>
    {
        [ForeignKey(nameof(Test))]
        public long TestId { get; set; }

        public virtual Test Test { get; set; }

        [ForeignKey(nameof(Question))]
        public long QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}
