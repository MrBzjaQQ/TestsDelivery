using System.ComponentModel.DataAnnotations.Schema;
using TestsDelivery.DAL.Shared.Models;

namespace TestsDelivery.DAL.Models.Questions
{
    public record AnswerOption : IdEntity<long>
    {
        public string Text { get; set; }

        public bool IsCorrect { get; set; }

        [ForeignKey(nameof(Questions.Question))]
        public long QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}
