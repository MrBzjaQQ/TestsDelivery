using System.ComponentModel.DataAnnotations.Schema;

namespace TestsPortal.DAL.Models.Questions
{
    public record ChoiceAnswer : AnswerBase
    {
        [ForeignKey(nameof(Questions.AnswerOption))]
        public long AnswerId { get; set; }

        public virtual AnswerOption AnswerOption { get; set; }
    }
}
