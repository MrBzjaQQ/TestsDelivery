using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.UserModels.Subject
{
    public record SubjectBaseModel
    {
        // TODO: LONG
        [Range(1, int.MaxValue)]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
