using System.ComponentModel.DataAnnotations.Schema;
using TestsDelivery.DAL.Shared.Models;

namespace TestsDelivery.DAL.Models.Questions
{
    public record Question : IdEntity<long>
    {
        public string Name { get; set; }

        public string Text { get; set; }

        public short Type { get; set; }

        [ForeignKey(nameof(Models.Subject.Subject))]
        public long SubjectId { get; set; }

        public virtual Subject.Subject Subject { get; set; }
    }
}
