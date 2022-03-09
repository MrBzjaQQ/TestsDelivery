namespace TestsPortal.Domain.Candidates
{
    public record Candidate
    {
        public long Id { get; set; }

        public long OriginalId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
