using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestsPortal.DAL.Models.Questions
{
    // TODO: problem here
    // Editing issue in admin panel
    // Probably versioning should solve the issue
    public record Question : IdEntity<long>
    {
        public string Name { get; set; } = string.Empty;

        public string Text { get; set; } = string.Empty;

        public short Type { get; set; }

        [ForeignKey(nameof(Models.Subject.Subject))]
        public long SubjectId { get; set; }

        public virtual Subject.Subject Subject { get; set; }
    }
}
