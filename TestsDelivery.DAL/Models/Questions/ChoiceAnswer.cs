using System.ComponentModel.DataAnnotations.Schema;

namespace TestsDelivery.DAL.Models.Questions
{
    public record ChoiceAnswer : AnswerBase
    {
        [ForeignKey(nameof(Questions.AnswerOption))]
        public long AnswerOptionId { get; set; }

        public virtual AnswerOption AnswerOption { get; set; }
    }
}
