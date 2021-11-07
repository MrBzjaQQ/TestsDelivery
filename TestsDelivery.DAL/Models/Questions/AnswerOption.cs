using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestsDelivery.DAL.Models.Questions
{
    public class AnswerOption
    {
        [Key]
        public long Id { get; set; }

        public string Text { get; set; }

        public bool IsCorrect { get; set; }

        [ForeignKey(nameof(Questions.Question))]
        public long QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}
