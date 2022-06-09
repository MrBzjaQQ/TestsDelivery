namespace TestsPortal.DAL.Models.Tests
{
    public record Test : IdOriginalIdEntity<long>
    {
        public string Name { get; set; }
    }
}
