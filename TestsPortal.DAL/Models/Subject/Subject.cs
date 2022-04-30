namespace TestsPortal.DAL.Models.Subject
{
    public record Subject : IdOriginalIdEntity<long>
    {
        public string Name { get; set; } = string.Empty;
    }
}
