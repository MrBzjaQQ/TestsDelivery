using System.ComponentModel.DataAnnotations;

namespace TestsDelivery.UserModels.Subject
{
    public record SubjectEditModel : SubjectBaseModel
    {
        [Required]
        public bool Retired { get; set; }
    }
}
