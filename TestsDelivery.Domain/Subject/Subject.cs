namespace TestsDelivery.Domain.Subject
{
    public record Subject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Retired { get; set; }
    }
}
