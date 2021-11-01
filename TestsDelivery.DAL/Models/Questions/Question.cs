using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestsDelivery.DAL.Models.Questions
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public short ItemType { get; set; }

        [ForeignKey(nameof(Models.Subject.Subject))]
        public int SubjectId { get; set; }

        public virtual Subject.Subject Subject { get; set; }
    }
}
