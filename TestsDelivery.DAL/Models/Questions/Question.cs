using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestsDelivery.DAL.Models.Questions
{
    public class Question
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public short ItemType { get; set; }

        [ForeignKey(nameof(Models.Subject.Subject))]
        public long SubjectId { get; set; }

        public virtual Subject.Subject Subject { get; set; }
    }
}
