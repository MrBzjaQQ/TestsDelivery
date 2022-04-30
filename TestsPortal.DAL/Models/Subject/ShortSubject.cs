namespace TestsPortal.DAL.Models.Subject
{
    public record ShortSubject : IdEntity<long>
    {
        public string Name { get; set; } = string.Empty;
    }
}
