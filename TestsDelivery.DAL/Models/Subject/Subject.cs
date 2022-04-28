namespace TestsDelivery.DAL.Models.Subject
{
    public record Subject : IdEntity<long>
    {
        public string Name { get; set; }

        public bool Retired { get; set; }
    }
}
