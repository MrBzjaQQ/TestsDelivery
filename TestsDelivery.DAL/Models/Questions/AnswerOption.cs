using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestsDelivery.DAL.Models.Questions
{
    public class AnswerOption
    {
        [Key]
        public int Id { get; set; }

        public string Text { get; set; }

        public bool IsCorrect { get; set; }

        [ForeignKey(nameof(Questions.Question))]
        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}
