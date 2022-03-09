using System.ComponentModel.DataAnnotations;

namespace TestsPortal.DAL.Models.Questions
{
    public record AnswerOption
    {
        [Key]
        public long Id { get; set; }

        public long OriginalId { get; set; }

        public string Text { get; set; } = string.Empty;
    }
}
