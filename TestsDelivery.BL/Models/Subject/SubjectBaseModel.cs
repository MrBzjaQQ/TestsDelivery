using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.BL.Models.Subject
{
    public record SubjectBaseModel
    {
        // TODO: LONG
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
