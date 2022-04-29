namespace TestsPortal.DAL.Models.Subject
{
    public record Subject : IdEntity<long>
    {
        public string Name { get; set; } = string.Empty;
    }
}
