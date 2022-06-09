using TestsPortal.Domain.Questions;

namespace TestsPortal.Domain.Tests
{
    public record Test
    {
        public long Id { get; set; }

        public long OriginalId { get; set; }

        public string Name { get; set; }

        public IEnumerable<QuestionBase> Questions { get; set; }
    }
}
