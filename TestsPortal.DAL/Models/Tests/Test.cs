namespace TestsPortal.DAL.Models.Tests
{
    public record Test : IdEntity<long>
    {
        public string Name { get; set; }
    }
}
