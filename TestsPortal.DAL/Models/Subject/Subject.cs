using System.ComponentModel.DataAnnotations;

namespace TestsPortal.DAL.Models.Subject
{
    public record Subject
    {
        [Key]
        public long Id { get; set; }

        public long OriginalId { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
