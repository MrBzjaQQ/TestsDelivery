using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.BL.Models.Subject
{
    public record SubjectCreateModel
    {
        [Required]
        public string Name { get; set; }
    }
}
