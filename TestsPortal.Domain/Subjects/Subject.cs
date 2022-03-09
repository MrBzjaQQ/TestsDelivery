namespace TestsPortal.Domain.Subjects
{
    public record Subject
    {
        public long Id { get; set; }

        public long OriginalId { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
