using System.ComponentModel.DataAnnotations.Schema;
using TestsDelivery.DAL.Models.Questions;
using TestsDelivery.DAL.Shared.Models;

namespace TestsDelivery.DAL.Models.Test
{
    public record QuestionInTest : IdEntity<long>
    {
        [ForeignKey(nameof(Test))]
        public long TestId { get; set; }

        public virtual Test Test { get; set; }

        [ForeignKey(nameof(Question))]
        public long QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}
