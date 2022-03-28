using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestsPortal.DAL.Models.Questions
{
    public record AnswerOption
    {
        [Key]
        public long Id { get; set; }

        public long OriginalId { get; set; }

        [ForeignKey(nameof(Question))]
        public long QuestionId { get; set; }

        public string Text { get; set; } = string.Empty;
    }
}
