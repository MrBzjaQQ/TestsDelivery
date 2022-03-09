using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestsPortal.DAL.Models.Questions;

namespace TestsPortal.DAL.Models.Tests
{
    public record QuestionInTest
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey(nameof(Test))]
        public long TestId { get; set; }

        public virtual Test Test { get; set; }

        [ForeignKey(nameof(Question))]
        public long QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}
