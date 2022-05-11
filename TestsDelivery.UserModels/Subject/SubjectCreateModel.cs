using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.UserModels.Subject
{
    public record SubjectCreateModel
    {
        [Required]
        public string Name { get; set; }
    }
}
