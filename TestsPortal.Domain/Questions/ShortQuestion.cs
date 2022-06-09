using TestsPortal.Domain.Subjects;

namespace TestsPortal.Domain.Questions
{
    public record ShortQuestion
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public ShortSubject Subject { get; set; }
    }
}
