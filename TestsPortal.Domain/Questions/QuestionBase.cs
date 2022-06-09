using TestsPortal.Domain.Subjects;

namespace TestsPortal.Domain.Questions
{
    public record QuestionBase
    {
        public long Id { get; set; }

        public long OriginalId { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public Subject Subject { get; set; }

        public QuestionType Type { get; set; }
    }
}
